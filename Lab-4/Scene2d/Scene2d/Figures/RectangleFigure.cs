namespace Scene2d.Figures
{
    using System.Drawing;
    using System.Collections.Generic;

    public class RectangleFigure : IFigure
    {
        /* Store four rectangle points because after rotation edges could be not parallel to XY axes.*/
        private ScenePoint _p1;
        private ScenePoint _p2;
        private ScenePoint _p3;
        private ScenePoint _p4;
        private ScenePoint[] _points = new ScenePoint[4];

        public RectangleFigure(ScenePoint p1, ScenePoint p2)
        {
            _p1 = p1;
            _p2 = new ScenePoint { X = p2.X, Y = p1.Y };
            _p3 = p2;
            _p4 = new ScenePoint { X = p1.X, Y = p2.Y };
            SetPoints(_p1, _p2, _p3, _p4);
        }

        //Ñopy constructor
        public RectangleFigure(ScenePoint p1, ScenePoint p2, ScenePoint p3, ScenePoint p4)
        {
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            SetPoints(_p1, _p2, _p3, _p4);
        }

        /* Ñalculate the rectangle that wraps current figure and has edges parallel to X and Y */
        public SceneRectangle CalculateCircumscribingRectangle()
        {
            var arrayX = new SortedSet<double> { _p1.X, _p2.X, _p3.X, _p4.X };
            var arrayY = new SortedSet<double> { _p1.Y, _p2.Y, _p3.Y, _p4.Y };

            var ñircumscribingRectangle = new SceneRectangle
            {
                Vertex1 = new ScenePoint { X = arrayX.Min, Y = arrayY.Max },
                Vertex2 = new ScenePoint { X = arrayX.Max, Y = arrayY.Min }
            };

            return ñircumscribingRectangle;
        }

        /* Return new Rectangle with the same points as the current one. */
        public object Clone()
        {
            var p1 = new ScenePoint { X = _p1.X, Y = _p1.Y };
            var p2 = new ScenePoint { X = _p2.X, Y = _p2.Y };
            var p3 = new ScenePoint { X = _p3.X, Y = _p3.Y };
            var p4 = new ScenePoint { X = _p4.X, Y = _p4.Y };
            var clone = new RectangleFigure(p1, p2, p3, p4);

            return clone;
        }

        /* Move all the points of current rectangle. */
        public void Move(ScenePoint vector)
        {
            GeneralMethodsFigure.Move(vector, ref _points);
            SetP(_points);
        }

        /* Rotate current rectangle. Rotation origin point is the rectangle center.*/
        public void Rotate(double angle)
        {
            GeneralMethodsFigure.RotateFigure(angle, CenterFigure(), ref _points);
            SetP(_points);
        }

        /* Reflect the figure. Reflection edge is the rectangle axis of symmetry (horizontal or vertical). */
        public void Reflect(ReflectOrientation orientation)
        {
            GeneralMethodsFigure.ReflectFigure(orientation, CenterFigure(), ref _points);
            SetP(_points);
        }

        public ScenePoint[] Points
        {
            get { return _points; }
            set
            {
                _points = value;
                SetP(_points);
            }
        }

        public void Draw(ScenePoint origin, Graphics drawing)
        {
            using (var pen = new Pen(Color.Blue))
            {
                drawing.DrawLine(
                    pen,
                    (float)(_p1.X - origin.X),
                    (float)(_p1.Y - origin.Y),
                    (float)(_p2.X - origin.X),
                    (float)(_p2.Y - origin.Y));

                drawing.DrawLine(
                    pen,
                    (float)(_p2.X - origin.X),
                    (float)(_p2.Y - origin.Y),
                    (float)(_p3.X - origin.X),
                    (float)(_p3.Y - origin.Y));

                drawing.DrawLine(
                    pen,
                    (float)(_p3.X - origin.X),
                    (float)(_p3.Y - origin.Y),
                    (float)(_p4.X - origin.X),
                    (float)(_p4.Y - origin.Y));

                drawing.DrawLine(
                    pen,
                    (float)(_p4.X - origin.X),
                    (float)(_p4.Y - origin.Y),
                    (float)(_p1.X - origin.X),
                    (float)(_p1.Y - origin.Y));
            }
        }

        public ScenePoint CenterFigure()
        {
            var arrayX = new SortedSet<double> { _p1.X, _p2.X, _p3.X, _p4.X };
            var arrayY = new SortedSet<double> { _p1.Y, _p2.Y, _p3.Y, _p4.Y };
            var x0 = (arrayX.Max + arrayX.Min) / 2.0;
            var y0 = (arrayY.Max + arrayY.Min) / 2.0;

            return new ScenePoint { X = x0, Y = y0 };
        }

        public void SetPoints(ScenePoint p1, ScenePoint p2, ScenePoint p3, ScenePoint p4)
        {
            _points[0] = p1;
            _points[1] = p2;
            _points[2] = p3;
            _points[3] = p4;
        }
        public void SetP(ScenePoint[] points)
        {
            _p1.X = points[0].X;
            _p1.Y = points[0].Y;
            _p2.X = points[1].X;
            _p2.Y = points[1].Y;
            _p3.X = points[2].X;
            _p3.Y = points[2].Y;
            _p4.X = points[3].X;
            _p4.Y = points[3].Y;
        }
    }
}
