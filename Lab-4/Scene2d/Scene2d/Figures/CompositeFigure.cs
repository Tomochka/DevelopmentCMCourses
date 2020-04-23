namespace Scene2d.Figures
{
    using System.Collections.Generic;
    using System.Drawing;

    public class CompositeFigure : ICompositeFigure
    {
        private List<IFigure> _childFigures = new List<IFigure>();
        private ScenePoint[] _points;

        public CompositeFigure(List<IFigure> figures)
        {
            foreach (var figure in figures)
            {
                _childFigures.Add(figure);
            }
        }

        public SceneRectangle CalculateCircumscribingRectangle()
        {
            var arrayX = new SortedSet<double>();
            var arrayY = new SortedSet<double>();

            foreach (var figure in _childFigures)
            {
                var сircRectangle = new SceneRectangle
                {
                    Vertex1 = figure.CalculateCircumscribingRectangle().Vertex1,
                    Vertex2 = figure.CalculateCircumscribingRectangle().Vertex2
                };

                arrayX.Add(сircRectangle.Vertex1.X);
                arrayX.Add(сircRectangle.Vertex2.X);
                arrayY.Add(сircRectangle.Vertex1.Y);
                arrayY.Add(сircRectangle.Vertex2.Y);
            }

            var сircumscribingRectangle = new SceneRectangle
            {
                Vertex1 = new ScenePoint { X = arrayX.Min, Y = arrayY.Max },
                Vertex2 = new ScenePoint { X = arrayX.Max, Y = arrayY.Min }
            };

            return сircumscribingRectangle;
        }

        public object Clone()
        {
            List<IFigure> figuresCloneList = new List<IFigure>();

            foreach (var figure in _childFigures)
            {
                IFigure figureClone = (IFigure)figure.Clone();
                figuresCloneList.Add(figureClone);
            }

            var clone = new CompositeFigure(figuresCloneList);

            return clone;
        }

        public void Move(ScenePoint vector)
        {
            foreach (var figure in _childFigures)
            {
                figure.Move(vector);
            }
        }

        public void Rotate(double angle)
        {
            foreach (var figure in _childFigures)
            {
                figure.Rotate(angle);
            }
        }

        public void Reflect(ReflectOrientation orientation)
        {
            SceneRectangle circumscribingRectangle = CalculateCircumscribingRectangle();
            var pointSceneCenter = new ScenePoint
            {
                X = (circumscribingRectangle.Vertex1.X + circumscribingRectangle.Vertex2.X) / 2.0,
                Y = (circumscribingRectangle.Vertex1.Y + circumscribingRectangle.Vertex2.Y) / 2.0
            };

            foreach (var figure in _childFigures)
            {
                var points = figure.Points;
                GeneralMethodsFigure.ReflectFigure(orientation, pointSceneCenter, ref points);
                figure.Points = points;
            }
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
            foreach (var figure in _childFigures)
            {
                figure.Draw(origin, drawing);
            }
        }

        public List<IFigure> ChildFigures
        {
            get { return _childFigures; }
        }
    }
}
