namespace Scene2d
{
    using System.Collections.Generic;
    using System.Linq;
    using Scene2d.Figures;
    using Scene2d.Exceptions;

    public class Scene
    {
        private readonly Dictionary<string, IFigure> _figures = new Dictionary<string, IFigure>();
        private readonly Dictionary<string, ICompositeFigure> _compositeFigures = new Dictionary<string, ICompositeFigure>();

        public Dictionary<string, IFigure> Figures
        {
            get
            {
                return _figures;
            }
        }

        public Dictionary<string, ICompositeFigure> CompositeFigures
        {
            get
            {
                return _compositeFigures;
            }
        }

        public void AddFigure(string name, IFigure figure)
        {
            if (!_figures.ContainsKey(name) && !_compositeFigures.ContainsKey(name))
            {
                _figures.Add(name, figure);
            }
            else throw new NameDoesAlreadyExist();
        }

        public SceneRectangle CalculateSceneCircumscribingRectangle()
        {
            var arrayX = new SortedSet<double>();
            var arrayY = new SortedSet<double>();

            foreach (var figure in _figures)
            {
                SceneRectangle figureSceneRectangle = figure.Value.CalculateCircumscribingRectangle();

                var ñircRectangle = new SceneRectangle
                {
                    Vertex1 = figureSceneRectangle.Vertex1,
                    Vertex2 = figureSceneRectangle.Vertex2
                };

                arrayX.Add(ñircRectangle.Vertex1.X);
                arrayX.Add(ñircRectangle.Vertex2.X);
                arrayY.Add(ñircRectangle.Vertex1.Y);
                arrayY.Add(ñircRectangle.Vertex2.Y);
            }

            foreach (var compositeFigure in _compositeFigures)
            {
                var ñircRectangle = new SceneRectangle
                {
                    Vertex1 = compositeFigure.Value.CalculateCircumscribingRectangle().Vertex1,
                    Vertex2 = compositeFigure.Value.CalculateCircumscribingRectangle().Vertex2
                };

                arrayX.Add(ñircRectangle.Vertex1.X);
                arrayX.Add(ñircRectangle.Vertex2.X);
                arrayY.Add(ñircRectangle.Vertex1.Y);
                arrayY.Add(ñircRectangle.Vertex2.Y);
            }

            var ñircumscribingRectangle = new SceneRectangle
            {
                Vertex1 = new ScenePoint { X = arrayX.Min, Y = arrayY.Max },
                Vertex2 = new ScenePoint { X = arrayX.Max, Y = arrayY.Min }
            };

            return ñircumscribingRectangle;
        }

        /* Create a group figure*/
        public void CreateCompositeFigure(string nameGroup, IEnumerable<string> childFigures)
        {
            var figuresList = new List<IFigure>();

            if (_compositeFigures.ContainsKey(nameGroup)) throw new NameDoesAlreadyExist();

            foreach (var nameFigure in childFigures)
            {
                if (_figures.ContainsKey(nameFigure) && !_compositeFigures.ContainsKey(nameFigure))
                {
                    figuresList.Add(_figures[nameFigure]);
                    _figures.Remove(nameFigure); //Delete after grouping
                }
                else
                {
                    throw new NameDoesAlreadyExist();
                }
            }

            var copositeFigure = new CompositeFigure(figuresList);
            _compositeFigures.Add(nameGroup, copositeFigure);
        }

        /* Calculate the rectangle that wraps figure or group 'name'*/
        public SceneRectangle CalculateCircumscribingRectangle(string name)
        {
            IFigure figure;
            ICompositeFigure compositeFigure;

            if (_figures.TryGetValue(name, out figure))
            {
                var point1 = new ScenePoint
                {
                    X = figure.CalculateCircumscribingRectangle().Vertex1.X,
                    Y = figure.CalculateCircumscribingRectangle().Vertex1.Y,
                };

                var point2 = new ScenePoint
                {
                    X = figure.CalculateCircumscribingRectangle().Vertex2.X,
                    Y = figure.CalculateCircumscribingRectangle().Vertex2.Y,
                };

                var ñircumscribingRectangle = new SceneRectangle
                {
                    Vertex1 = point1,
                    Vertex2 = point2
                };

                return ñircumscribingRectangle;
            }
            else if (_compositeFigures.TryGetValue(name, out compositeFigure))
            {

                var point1 = new ScenePoint
                {
                    X = compositeFigure.CalculateCircumscribingRectangle().Vertex1.X,
                    Y = compositeFigure.CalculateCircumscribingRectangle().Vertex1.Y,
                };

                var point2 = new ScenePoint
                {
                    X = compositeFigure.CalculateCircumscribingRectangle().Vertex2.X,
                    Y = compositeFigure.CalculateCircumscribingRectangle().Vertex2.Y,
                };

                var ñircumscribingRectangle = new SceneRectangle
                {
                    Vertex1 = point1,
                    Vertex2 = point2
                };

                return ñircumscribingRectangle;
            }
            else
            {
                throw new BadName();
            }
        }

        /* Move all the figures and groups in the scene by 'vector'.*/
        public void MoveScene(ScenePoint vector)
        {
            foreach (var figure in _figures)
            {
                var points = figure.Value.Points;
                GeneralMethodsFigure.Move(vector, ref points);
                figure.Value.Points = points;
            }

            foreach (var composite in _compositeFigures)
            {
                foreach (var compositeFigures in composite.Value.ChildFigures)
                {
                    var points = compositeFigures.Points;
                    GeneralMethodsFigure.Move(vector, ref points);
                    compositeFigures.Points = points;
                }
            }
        }

        /* Move figure or group 'name' by 'vector'*/
        public void Move(string name, ScenePoint vector)
        {
            IFigure figure;
            ICompositeFigure compositeFigure;

            if (_figures.TryGetValue(name, out figure))
            {
                figure.Move(vector);
            }
            else if (_compositeFigures.TryGetValue(name, out compositeFigure))
            {
                compositeFigure.Move(vector);
            }
            else
            {
                throw new BadName();
            }

        }

        /* Rotate all figures and groups in the scene by 'angle'*/
        public void RotateScene(double angle)
        {
            SceneRectangle sceneRectangle = CalculateSceneCircumscribingRectangle();
            var pointSceneCenter = new ScenePoint
            {
                X = (sceneRectangle.Vertex1.X + sceneRectangle.Vertex2.X) / 2.0,
                Y = (sceneRectangle.Vertex1.Y + sceneRectangle.Vertex2.Y) / 2.0
            };

            foreach (var figure in _figures)
            {
                var points = figure.Value.Points;
                GeneralMethodsFigure.RotateFigure(angle, pointSceneCenter, ref points);
                figure.Value.Points = points;
            }

            foreach (var composite in _compositeFigures)
            {
                foreach (var compositeFigures in composite.Value.ChildFigures)
                {
                    var points = compositeFigures.Points;
                    GeneralMethodsFigure.RotateFigure(angle, pointSceneCenter, ref points);
                    compositeFigures.Points = points;
                }
            }
        }

        /* Should rotate figure or group 'name' by 'angle'*/
        public void Rotate(string name, double angle)
        {
            IFigure figure;
            ICompositeFigure compositeFigure;

            if (_figures.TryGetValue(name, out figure))
            {
                figure.Rotate(angle);
            }
            else if (_compositeFigures.TryGetValue(name, out compositeFigure))
            {
                compositeFigure.Rotate(angle);
            }
            else
            {
                throw new BadName();
            }
        }

        public IEnumerable<IFigure> ListDrawableFigures()
        {
            return _figures
                .Values
                .Concat(_compositeFigures.SelectMany(x => x.Value.ChildFigures))
                .Distinct();
        }

        /*Copy the entire scene to a group named 'copyName'*/
        public void CopyScene(string copyName)
        {
            List<IFigure> figuresScene = new List<IFigure>(); // List of all figures that are in the _compositeFigures and in _figures

            foreach (var figure in _figures)
            {
                figuresScene.Add(figure.Value);
            }

            foreach (var compositeFigure in _compositeFigures)
            {
                List<IFigure> figuresComposite = compositeFigure.Value.ChildFigures;

                foreach (var figureComposite in figuresComposite)
                {
                    figuresScene.Add(figureComposite);
                }
            }

            _compositeFigures.Add(copyName, new CompositeFigure(figuresScene));
        }

        /* Copy figure or group 'originalName' to 'copyName'*/
        public void Copy(string originalName, string copyName)
        {
            IFigure figure;
            ICompositeFigure compositeFigure;

            if (_figures.ContainsKey(copyName) || _compositeFigures.ContainsKey(copyName))
            {
                throw new NameDoesAlreadyExist();
            }

            if (_figures.TryGetValue(originalName, out figure))
            {
                _figures.Add(copyName, (IFigure)figure.Clone());
            }
            else if (_compositeFigures.TryGetValue(originalName, out compositeFigure))
            {
                _compositeFigures.Add(copyName, (ICompositeFigure)compositeFigure.Clone());
            }
            else
            {
                throw new BadName();
            }
        }

        /* Delete all the figures and groups from the scene.*/
        public void DeleteScene()
        {
            _figures.Clear();
            _compositeFigures.Clear();
        }

        /* Delete figure or group.*/
        public void Delete(string name)
        {
            if (_figures.ContainsKey(name))
            {
                _figures.Remove(name);
            }
            else if (_compositeFigures.ContainsKey(name))
            {
                _compositeFigures.Remove(name);
            }
            else throw new BadName();
        }

        /* Reflect all the figures and groups in the scene.*/
        public void ReflectScene(ReflectOrientation reflectOrientation)
        {
            SceneRectangle sceneRectangle = CalculateSceneCircumscribingRectangle();
            var pointSceneCenter = new ScenePoint
            {
                X = (sceneRectangle.Vertex1.X + sceneRectangle.Vertex2.X) / 2.0,
                Y = (sceneRectangle.Vertex1.Y + sceneRectangle.Vertex2.Y) / 2.0
            };

            foreach (var figure in _figures)
            {
                var points = figure.Value.Points;
                GeneralMethodsFigure.ReflectFigure(reflectOrientation, pointSceneCenter, ref points);
                figure.Value.Points = points;
            }


            foreach (var composite in _compositeFigures)
            {
                foreach (var compositeFigures in composite.Value.ChildFigures)
                {
                    var points = compositeFigures.Points;
                    GeneralMethodsFigure.ReflectFigure(reflectOrientation, pointSceneCenter, ref points);
                    compositeFigures.Points = points;
                }
            }
        }

        /* Reflect figure or group 'name'.*/
        public void Reflect(string name, ReflectOrientation reflectOrientation)
        {
            IFigure figure;
            ICompositeFigure compositeFigure;

            if (_figures.TryGetValue(name, out figure))
            {
                figure.Reflect(reflectOrientation);
            }
            else if (_compositeFigures.TryGetValue(name, out compositeFigure))
            {
                compositeFigure.Reflect(reflectOrientation);
            }
            else
            {
                throw new BadName();
            }
        }
    }

}
