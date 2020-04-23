namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Figures;
    using Scene2d.Exceptions;
    using System;
    using System.Collections.Generic;

    class AddPolygonCommandBuilder : ICommandBuilder
    {
        private const double Eps = 1e-10;
        const string name = @"\s(\d|\w|-|_){1,}";
        const string point = @"\(-?\d{1,},\s?-?\d{1,}\)";

        private static readonly Regex RecognizeRegexStart = new Regex(@"^add polygon");
        private static readonly Regex RecognizeRegexCoordinate = new Regex(@"\s{0,}add point " + point + @"$");
        private static readonly Regex RecognizeRegexEnd = new Regex(@"^end polygon$");
        private IFigure _polygon;
        private string _name;
        private bool _endPoligon = false;
        private List<ScenePoint> _coordinatePolygon = new List<ScenePoint>();

        public bool IsCommandReady
        {
            get
            {
                if (_endPoligon) return true;
                else throw new UnexpectedEndOfPolygon();
            }
        }

        public void AppendLine(string line)
        {
            var matchStart = RecognizeRegexStart.Match(line);
            var matchCoordinate = RecognizeRegexCoordinate.Match(line);
            var matchEnd = RecognizeRegexEnd.Match(line);

            if (matchStart.Success)
            {
                line = line.Remove(matchStart.Index, matchStart.Length);
                _name = line.Trim();
            }
            else if (matchCoordinate.Success)
            {
                var coordinate = new double[2];
                var i = 0;

                foreach (var valueCoordinate in Regex.Matches(matchCoordinate.ToString(), @"-?\d{1,}"))
                {
                    if (valueCoordinate.ToString().Length > 1 && valueCoordinate.ToString()[0] == '0')
                    {
                        throw new BadFormatException();
                    }

                    coordinate[i] = double.Parse(valueCoordinate.ToString());
                    i++;
                }

                var coord = new ScenePoint { X = coordinate[0], Y = coordinate[1] };

                //Check for a match with a point in the polygon
                for (var j = 0; j < _coordinatePolygon.Count; j++)
                {
                    if (Math.Abs(_coordinatePolygon[j].X - coord.X) < Eps && Math.Abs(_coordinatePolygon[j].Y - coord.Y) < Eps)
                    {
                        throw new BadPolygonPoint();
                    }
                }

                //Self-healing check
                if (_coordinatePolygon.Count >= 2)
                {
                    for (var j = 0; j < _coordinatePolygon.Count - 1; j++)
                    {
                        if (IsSegmentsIntersect(_coordinatePolygon[j], _coordinatePolygon[j + 1],
                            _coordinatePolygon[_coordinatePolygon.Count - 1], coord))
                        {
                            throw new BadPolygonPoint();
                        }
                    }
                }

                _coordinatePolygon.Add(coord);
            }
            else if (matchEnd.Success)
            {

                if (_coordinatePolygon.Count < 3)
                {
                    throw new BadPolygonPointNumber();
                }

                var coordinates = new ScenePoint[_coordinatePolygon.Count];

                for (var i = 0; i < coordinates.Length; i++)
                {
                    coordinates[i] = _coordinatePolygon[i];
                }

                _endPoligon = true;
                _polygon = new PolygonFigure(coordinates);
            }
            else
            {
                throw new BadFormatException();
            }
        }

        public ICommand GetCommand() => new AddFigureCommand(_name, _polygon);

        private static bool IsSegmentsIntersect(ScenePoint a, ScenePoint b, ScenePoint c, ScenePoint d)
        {
            if (Math.Abs(b.X - c.X) < Eps && Math.Abs(b.Y - c.Y) < Eps)
            {
                var k1 = (b.X - a.X) / (b.Y - a.Y);
                var k2 = (d.X - c.X) / (d.Y - c.Y);

                if (k1 != k2) return false;
            }

            return Intersect(a.X, b.X, c.X, d.X)
                   && Intersect(a.Y, b.Y, c.Y, d.Y)
                   && OrientedAreaTriangle(a, b, c) * OrientedAreaTriangle(a, b, d) <= Eps
                   && OrientedAreaTriangle(c, d, a) * OrientedAreaTriangle(c, d, b) <= Eps;
        }
        private static bool Intersect(double a, double b, double c, double d)
        {
            if (a > b)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }

            if (c > d)
            {
                var tmp = c;
                c = d;
                d = tmp;
            }

            return Math.Max(a, c) <= Math.Min(b, d);
        }

        private static double OrientedAreaTriangle(ScenePoint a, ScenePoint b, ScenePoint c)
        {
            return ((b.X - a.X) * (c.Y - a.Y)) - ((b.Y - a.Y) * (c.X - a.X));
        }

    }
}
