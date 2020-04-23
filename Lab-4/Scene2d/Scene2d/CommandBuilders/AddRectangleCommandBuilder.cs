namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Figures;
    using Scene2d.Exceptions;
    using System;

    public class AddRectangleCommandBuilder : ICommandBuilder
    {
        private const double Eps = 1e-10;
        const string name = @"(\d|\w|-|_){1,}";
        const string point = @"\(-?\d{1,},\s?-?\d{1,}\)";
        private static readonly Regex RecognizeRegex = new Regex(name + @"\s" + point + @"\s" + point + @"$");
        private string _name;
        private IFigure _rectangle;

        public bool IsCommandReady
        {
            get
            {
                return true;
            }
        }

        public void AppendLine(string line)
        {
            var match = RecognizeRegex.Match(line);

            if (match.Success)
            {
                line = match.ToString();
                _name = Regex.Match(line, name).ToString().Trim();
                line = line.Remove(0, _name.Length).Trim();
                
                var coordinate = new double[2, 2];
                var i = 0;

                foreach (var coordinateMatch in Regex.Matches(line, point))
                {
                    var j = 0;

                    foreach (var valueCoordinate in Regex.Matches(coordinateMatch.ToString(), @"-?\d{1,}"))
                    {
                        if (valueCoordinate.ToString().Length > 1 && valueCoordinate.ToString()[0] == '0')
                        {

                            throw new BadFormatException();
                        }

                        coordinate[i, j] = double.Parse(valueCoordinate.ToString());
                        j++;
                    }

                    i++;
                }

                var p1 = new ScenePoint { X = coordinate[0, 0], Y = coordinate[0, 1] };
                var p2 = new ScenePoint { X = coordinate[1, 0], Y = coordinate[1, 1] };

                if (Math.Abs(p1.X - p2.X) < Eps || Math.Abs(p1.Y - p2.Y) < Eps) 
                {
                    throw new BadRectanglePoint();
                }

                _rectangle = new RectangleFigure(p1, p2);
            }
            else
            {
                throw new BadFormatException();
            }
        }

        public ICommand GetCommand() => new AddFigureCommand(_name, _rectangle);
    }
}