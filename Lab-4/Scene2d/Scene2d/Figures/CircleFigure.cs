namespace Scene2d.Figures
{
    using System.Drawing;
    using System.Collections.Generic;

    public class CircleFigure : IFigure
    {
        private ScenePoint _center;
        private double _radius;
        public ScenePoint[] _points = new ScenePoint[1];

        public CircleFigure(ScenePoint center, double radius)
        {
            _center = center;
            _radius = radius;
            _points[0] = _center;
        }

        public SceneRectangle CalculateCircumscribingRectangle()
        {
            var ñircumscribingRectangle = new SceneRectangle
            {
                Vertex1 = new ScenePoint { X = _center.X - _radius, Y = _center.Y + _radius },
                Vertex2 = new ScenePoint { X = _center.X + _radius, Y = _center.Y - _radius }
            };

            return ñircumscribingRectangle;
        }

        public object Clone()
        {
            var center = new ScenePoint { X = _center.X, Y = _center.Y };
            var clone = new CircleFigure(center, _radius);

            return clone;
        }

        public void Move(ScenePoint vector)
        {
            GeneralMethodsFigure.Move(vector, ref _points);
            _center = _points[0];
        }

        public void Rotate(double angle)
        {
            //Do nothing
        }

        public void Reflect(ReflectOrientation orientation)
        {
            //Do nothing
        }

        public ScenePoint[] Points
        {
            get { return _points; }
            set
            {
                _points = value;
                _center = _points[0];
            }
        }

        public void Draw(ScenePoint origin, Graphics drawing)
        {
            using (var pen = new Pen(Color.Green))
            {
                drawing.DrawEllipse(
                    pen,
                    (int)(_center.X - _radius - origin.X),
                    (int)(_center.Y - _radius - origin.Y),
                    (int)(_radius * 2),
                    (int)(_radius * 2));
            }
        }
    }
}