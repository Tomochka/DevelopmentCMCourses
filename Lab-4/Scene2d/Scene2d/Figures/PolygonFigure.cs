namespace Scene2d.Figures
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Collections.Generic;
    using Scene2d;

    public class PolygonFigure : IFigure
    {
        private ScenePoint[] _points;

        public PolygonFigure(ScenePoint[] points)
        {
            _points = points;
        }

        public SceneRectangle CalculateCircumscribingRectangle()
        {
            var arrayX = new SortedSet<double>();
            var arrayY = new SortedSet<double>();

            foreach (var point in _points)
            {
                arrayX.Add(point.X);
                arrayY.Add(point.Y);
            }

            var ñircumscribingRectangle = new SceneRectangle
            {
                Vertex1 = new ScenePoint { X = arrayX.Min, Y = arrayY.Max },
                Vertex2 = new ScenePoint { X = arrayX.Max, Y = arrayY.Min }
            };

            return ñircumscribingRectangle;
        }

        public object Clone()
        {
            var points = new ScenePoint[_points.Length];
            var i = 0;

            foreach (var point in _points)
            {
                points[i] = new ScenePoint { X = point.X, Y = point.Y };
                i++;
            }

            var clone = new PolygonFigure(points);

            return clone;
        }
        public void Move(ScenePoint vector)
        {
            GeneralMethodsFigure.Move(vector, ref _points);
        }

        public void Rotate(double angle)
        {
            GeneralMethodsFigure.RotateFigure(angle, CenterFigure(), ref _points);
        }

        public void Reflect(ReflectOrientation orientation)
        {
            GeneralMethodsFigure.ReflectFigure(orientation, CenterFigure(), ref _points);
        }

        public ScenePoint[] Points
        {
            get { return _points; }
            set
            {
                _points = value;
            }
        }

        public void Draw(ScenePoint origin, Graphics drawing)
        {
            using (var pen = new Pen(Color.DarkOrchid))
            {
                for (var i = 0; i < _points.Length; i++)
                {
                    ScenePoint firstPoint = _points[i];
                    ScenePoint secondPoint = i >= _points.Length - 1 ? _points.First() : _points[i + 1];

                    drawing.DrawLine(
                        pen,
                        (float)(firstPoint.X - origin.X),
                        (float)(firstPoint.Y - origin.Y),
                        (float)(secondPoint.X - origin.X),
                        (float)(secondPoint.Y - origin.Y));
                }
            }
        }

        public ScenePoint CenterFigure()
        {
            var x0 = 0.0;
            var y0 = 0.0;
            var area = 0.0;
            var points = new List<ScenePoint>(_points);

            points.Add(_points[0]);

            for (var i = 0; i < points.Count - 1; i++)
            {
                area += points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y;
                x0 += (points[i].X + points[i + 1].X) * (points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y);
                y0 += (points[i].Y + points[i + 1].Y) * (points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y);
            }

            area = area / 2.0;
            x0 = x0 / (6 * area);
            y0 = y0 / (6 * area);

            return new ScenePoint { X = x0, Y = y0 };
        }
    }
}