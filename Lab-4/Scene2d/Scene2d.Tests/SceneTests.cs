namespace Scene2d.Tests
{
    using NUnit.Framework;
    using Scene2d.CommandBuilders;
    using Scene2d.Figures;
    using System;

    [TestFixture]
    class SceneTests
    {
        [Test]
        public void Scene_AddRectangle_ShouldBeСorrectAdd()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-4, -3) (2, 6)");

            Assert.That(commandProducer.IsCommandReady,
                Is.True,
                "Правильная команда принялась за неправильную");

            var command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("R1"),
                Is.True,
                "Прямоугольник не был добавлен");

            Assert.That(scene.Figures["R1"].GetType(),
                Is.EqualTo(typeof(RectangleFigure)),
                "Фигура оказалась не прямоугольником");

            bool matching = false;
            ScenePoint[] points = scene.Figures["R1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(-4, -3)) &&
                EqualScenePoints(points[1], new ScenePoint(2, -3)) &&
                EqualScenePoints(points[2], new ScenePoint(2, 6)) &&
                EqualScenePoints(points[3], new ScenePoint(-4, 6)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника неверные");
        }

        [Test]
        public void Scene_AddCircle_ShouldBeСorrectAdd()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-4, -3) radius 5");

            Assert.That(commandProducer.IsCommandReady,
                Is.True,
                "Правильная команда принялась за неправильную");

            var command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("C1"),
                Is.True,
                "Окружность не была добавлена");

            Assert.That(scene.Figures["C1"].GetType(),
                Is.EqualTo(typeof(CircleFigure)),
                "Фигура оказалась не окружностью");

            bool matching = false;
            ScenePoint[] points = scene.Figures["C1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(-4, -3)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты центра окружности неверные");
        }

        [Test]
        public void Scene_AddPolygon_ShouldBeСorrectAdd()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-3, 1)");
            commandProducer.AppendLine("add point (1, 5)");
            commandProducer.AppendLine("add point (0, 0)");
            commandProducer.AppendLine("add point (7, -6)");
            commandProducer.AppendLine("add point (-1, -4)");
            commandProducer.AppendLine("end polygon");

            Assert.That(commandProducer.IsCommandReady,
                Is.True,
                "Правильный набор команд принялся за неправильный");

            var command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("P1"),
                Is.True,
                "Полигон не был добавлен");

            Assert.That(scene.Figures["P1"].GetType(),
                Is.EqualTo(typeof(PolygonFigure)),
                "Фигура оказалась не полигоном");

            bool matching = false;
            ScenePoint[] points = scene.Figures["P1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(-3, 1)) &&
                EqualScenePoints(points[1], new ScenePoint(1, 5)) &&
                EqualScenePoints(points[2], new ScenePoint(0, 0)) &&
                EqualScenePoints(points[3], new ScenePoint(7, -6)) &&
                EqualScenePoints(points[4], new ScenePoint(-1, -4)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты полигона неверные");
        }

        [Test]
        public void Scene_AddCompositeFigures_ShouldBeСorrectAdd()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.CompositeFigures.ContainsKey("G1"),
                Is.True,
                "Фигуры не были сгруппированы");

            bool matchingFigures = false;

            if (scene.CompositeFigures["G1"].ChildFigures[0].GetType() == typeof(CircleFigure) &&
                scene.CompositeFigures["G1"].ChildFigures[1].GetType() == typeof(PolygonFigure))
            {
                matchingFigures = true;
            }

            Assert.That(matchingFigures,
                Is.True,
                "Композиция фигур состоит из неправильных фигур");
            
            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.CompositeFigures["G1"].ChildFigures[0].Points;
            ScenePoint[] pointsFigure2 = scene.CompositeFigures["G1"].ChildFigures[1].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -2)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(0, 2)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(-2, -1)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(2, -1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты фигур в группе неверные");
        }

        [Test]
        public void Scene_CopyRectangle_ShouldBeСorrectCopy()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-4, -3) (2, 6)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("copy R1 to R2");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("R2"),
                Is.True,
                "Прямоугольник не был скопирован");

            Assert.That(scene.Figures["R2"].GetType(),
                Is.EqualTo(typeof(RectangleFigure)),
                "Скопированная фигура оказалась не прямоугольником");

            bool matching = false;
            ScenePoint[] points = scene.Figures["R2"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(-4, -3)) &&
                EqualScenePoints(points[1], new ScenePoint(2, -3)) &&
                EqualScenePoints(points[2], new ScenePoint(2, 6)) &&
                EqualScenePoints(points[3], new ScenePoint(-4, 6)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты скопированного прямоугольника неверные");
        }

        [Test]
        public void Scene_CopyCompositeFigures_ShouldBeСorrectCopy()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("copy G1 to G2");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.CompositeFigures.ContainsKey("G2"),
                Is.True,
                "Композиция фигур не была скопирована");

            bool matchingFigures = false;

            if (scene.CompositeFigures["G2"].ChildFigures[0].GetType() == typeof(CircleFigure) &&
                scene.CompositeFigures["G2"].ChildFigures[1].GetType() == typeof(PolygonFigure))
            {
                matchingFigures = true;
            }

            Assert.That(matchingFigures,
                Is.True,
                "Скопированная композиция фигур состоит из неправильных фигур");

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.CompositeFigures["G2"].ChildFigures[0].Points;
            ScenePoint[] pointsFigure2 = scene.CompositeFigures["G2"].ChildFigures[1].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -2)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(0, 2)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(-2, -1)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(2, -1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты фигур в группе неверные");
        }

        [Test]
        public void Scene_CopyScene_ShouldBeСorrectCopy()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add circle C2 (-7, 5) radius 1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add rectangle R1 (-6, 2) (-2, 7)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();

            commandProducer.AppendLine("copy scene to G2");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.CompositeFigures.ContainsKey("G2"),
                Is.True,
                "Сцена не была скопирована");
            
            bool matchingFigures = false;

            if (scene.CompositeFigures["G2"].ChildFigures[0].GetType() == typeof(CircleFigure) &&
                scene.CompositeFigures["G2"].ChildFigures[1].GetType() == typeof(CircleFigure) &&
                scene.CompositeFigures["G2"].ChildFigures[2].GetType() == typeof(PolygonFigure) &&
                scene.CompositeFigures["G2"].ChildFigures[3].GetType() == typeof(RectangleFigure))
            {
                matchingFigures = true;
            }

            Assert.That(matchingFigures,
                Is.True,
                "Скопированная сцена состоит из неправильных элементов");
        }

        [Test]
        public void Scene_DeleteRectangle_ShouldBeСorrectDelete()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-4, -3) (2, 6)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("delete R1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("R1"),
                Is.False,
                "Прямоугольник не был удален");
        }

        [Test]
        public void Scene_DeleteCompositeFigures_ShouldBeСorrectDelete()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("delete G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            Assert.That(scene.Figures.ContainsKey("G1"),
                Is.False,
                "Композиция фигур не была удалена");
        }

        [Test]
        public void Scene_DeleteScene_ShouldBeСorrectDelete()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add circle C2 (-7, 5) radius 1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add rectangle R1 (-6, 2) (-2, 7)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("delete scene");
            command = commandProducer.GetCommand();
            command.Apply(scene);


            bool matchingFigures = false;

            if (scene.Figures.ContainsKey("C2") || scene.Figures.ContainsKey("R1") || scene.CompositeFigures.ContainsKey("G1"))
            {
                matchingFigures = true;
            }

            Assert.That(matchingFigures,
                Is.False,
                "Сцена не была удалена");
        }

        [Test]
        public void Scene_MoveRectangle_ShouldBeСorrectMove()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-4, -3) (2, 6)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("move R1 (3, -2)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["R1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(-1, -5)) &&
                EqualScenePoints(points[1], new ScenePoint(5, -5)) &&
                EqualScenePoints(points[2], new ScenePoint(5, 4)) &&
                EqualScenePoints(points[3], new ScenePoint(-1, 4)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Перемещение прямоугольника на вектор произошло неверно");
        }
       
        [Test]
        public void Scene_MoveCircle_ShouldBeСorrectMove()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (2, -4) radius 5");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("move C1 (3, -2)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["C1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(5, -6)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Перемещение окружности на вектор произошло неверно");
        }

        [Test]
        public void Scene_MovePolygon_ShouldBeСorrectMove()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-3, 1)");
            commandProducer.AppendLine("add point (1, 5)");
            commandProducer.AppendLine("add point (0, 0)");
            commandProducer.AppendLine("add point (7, -6)");
            commandProducer.AppendLine("add point (-1, -4)");
            commandProducer.AppendLine("end polygon");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("move P1 (3, -2)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["P1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(0, -1)) &&
                EqualScenePoints(points[1], new ScenePoint(4, 3)) &&
                EqualScenePoints(points[2], new ScenePoint(3, -2)) &&
                EqualScenePoints(points[3], new ScenePoint(10, -8)) &&
                EqualScenePoints(points[4], new ScenePoint(2, -6)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Перемещение полигона на вектор произошло неверно");
        }

        [Test]
        public void Scene_MoveCompositeFigures_ShouldBeСorrectMove()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("move G1 (3, -2)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.CompositeFigures["G1"].ChildFigures[0].Points;
            ScenePoint[] pointsFigure2 = scene.CompositeFigures["G1"].ChildFigures[1].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(2, -4)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(3, 0)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(1, -3)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(5, -3)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Перемещение композиции фигур произошло неверно");
        }

        [Test]
        public void Scene_MoveScene_ShouldBeСorrectMove()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("move scene (3, -2)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.Figures["C1"].Points;
            ScenePoint[] pointsFigure2 = scene.Figures["P1"].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(2, -4)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(3, 0)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(1, -3)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(5, -3)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Перемещение сцены произошло неверно");
        }
        [Test]
        public void Scene_RotateRectangle_ShouldBeСorrectRotate()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-2, -5) (2, 5)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("rotate R1 90");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["R1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(5, -2)) &&
                EqualScenePoints(points[1], new ScenePoint(5, 2)) &&
                EqualScenePoints(points[2], new ScenePoint(-5, 2)) &&
                EqualScenePoints(points[3], new ScenePoint(-5, -2)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Вращение прямоугольника произошло неверно");
        }

        [Test]
        public void Scene_RotatePolygon_ShouldBeСorrectRotate()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("rotate P1 180");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["P1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(0, -2)) &&
                EqualScenePoints(points[1], new ScenePoint(2, 1)) &&
                EqualScenePoints(points[2], new ScenePoint(-2, 1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Вращение полигона произошло неверно");
        }

        [Test]
        public void Scene_RotateCompositeFigures_ShouldBeСorrectRotate()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("rotate G1 180");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.CompositeFigures["G1"].ChildFigures[0].Points;
            ScenePoint[] pointsFigure2 = scene.CompositeFigures["G1"].ChildFigures[1].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -2)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(0, -2)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(2, 1)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(-2, 1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Вращение композиции фигур произошло неверно");
        }

        [Test]
        public void Scene_RotateScene_ShouldBeСorrectRotate()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("rotate scene 180");
            command = commandProducer.GetCommand();
            command.Apply(scene);
            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.Figures["C1"].Points;
            ScenePoint[] pointsFigure2 = scene.Figures["P1"].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -1)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(-2, -5)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(0, -2)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(-4, -2)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Вращение сцены произошло неверно");
        }
        [Test]
        public void Scene_ReflectRectangleHorizontally_ShouldBeСorrectReflect()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (2, 1) (6, 5)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("reflect horizontally R1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["R1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(2, 5)) &&
                EqualScenePoints(points[1], new ScenePoint(6, 5)) &&
                EqualScenePoints(points[2], new ScenePoint(6, 1)) &&
                EqualScenePoints(points[3], new ScenePoint(2, 1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Отражение прямоугольника произошло неверно");
        }

        [Test]
        public void Scene_ReflectPolygonHorizontally_ShouldBeСorrectReflect()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("end polygon");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("reflect horizontally P1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["P1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(0, -2)) &&
                EqualScenePoints(points[1], new ScenePoint(2, 1)) &&
                EqualScenePoints(points[2], new ScenePoint(-2, 1)))
                {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Отражение полигона произошло неверно");
        }

        [Test]
        public void Scene_ReflectRectangleVertically_ShouldBeСorrectReflect()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (2, 1) (6, 5)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("reflect vertically R1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] points = scene.Figures["R1"].Points;

            if (EqualScenePoints(points[0], new ScenePoint(6, 1)) &&
                EqualScenePoints(points[1], new ScenePoint(2, 1)) &&
                EqualScenePoints(points[2], new ScenePoint(2, 5)) &&
                EqualScenePoints(points[3], new ScenePoint(6, 5)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Отражение прямоугольника произошло неверно");
        }

        [Test]
        public void Scene_ReflectCompositeFiguresVertically_ShouldBeСorrectReflect()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("reflect vertically G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.CompositeFigures["G1"].ChildFigures[0].Points;
            ScenePoint[] pointsFigure2 = scene.CompositeFigures["G1"].ChildFigures[1].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -2)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(-2, 2)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(0, -1)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(-4, -1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Отражение композиции фигур произошло неверно");
        }

        [Test]
        public void Scene_ReflectSceneVertically_ShouldBeСorrectReflect()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("reflect vertically scene");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            bool matching = false;
            ScenePoint[] pointsFigure1 = scene.Figures["C1"].Points;
            ScenePoint[] pointsFigure2 = scene.Figures["P1"].Points;

            if (EqualScenePoints(pointsFigure1[0], new ScenePoint(-1, -2)) &&
                EqualScenePoints(pointsFigure2[0], new ScenePoint(-2, 2)) &&
                EqualScenePoints(pointsFigure2[1], new ScenePoint(0, -1)) &&
                EqualScenePoints(pointsFigure2[2], new ScenePoint(-4, -1)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Отражение сцены произошло неверно");
        }

        [Test]
        public void Scene_СalculateCircumscribingRectangle_ShouldBeСorrectСalculateСoordinates()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (2, 1) (6, 5)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            SceneRectangle sceneRectangle = scene.CalculateCircumscribingRectangle("R1");
            bool matching = false;

            if (EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(2, 5)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(6, 1)) ||
                EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(2, 1)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(6, 5)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника, описывающего прямоугольник, неверные");
        }

        [Test]
        public void Scene_СalculateCircumscribingCircle_ShouldBeСorrectСalculateСoordinates()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-4, -3) radius 5");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            SceneRectangle sceneRectangle = scene.CalculateCircumscribingRectangle("C1");
            bool matching = false;

            if (EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-9, 2)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(1, -8)) ||
                EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(1, 2)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(-9, -8)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника, описывающего окружность, неверные");
        }

        [Test]
        public void Scene_СalculateCircumscribingPolygon_ShouldBeСorrectСalculateСoordinates()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-3, 1)");
            commandProducer.AppendLine("add point (1, 5)");
            commandProducer.AppendLine("add point (0, 0)");
            commandProducer.AppendLine("add point (7, -6)");
            commandProducer.AppendLine("add point (-1, -4)");
            commandProducer.AppendLine("end polygon");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            SceneRectangle sceneRectangle = scene.CalculateCircumscribingRectangle("P1");
            bool matching = false;

            if (EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-3, -6)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(7, 5)) ||
                EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-3, 5)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(7, -6)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника, описывающего полигон, неверные");
        }

        [Test]
        public void Scene_СalculateCircumscribingCompositeFigures_ShouldBeСorrectСalculateСoordinates()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            SceneRectangle sceneRectangle = scene.CalculateCircumscribingRectangle("G1");
            bool matching = false;

            if (EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-4, -5)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(2, 2)) ||
                EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-4, 2)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(2, -5)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника, описывающего композицию фигур, неверные");
        }

        [Test]
        public void Scene_СalculateCircumscribingScene_ShouldBeСorrectСalculateСoordinates()
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add circle C1 (-1, -2) radius 3");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add circle C2 (-7, 5) radius 1");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (0, 2)");
            commandProducer.AppendLine("add point (-2, -1)");
            commandProducer.AppendLine("add point (2, -1)");
            commandProducer.AppendLine("end polygon");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add rectangle R1 (-6, 2) (-2, 7)");
            command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("group C1, P1 as G1");
            command = commandProducer.GetCommand();

            SceneRectangle sceneRectangle = scene.CalculateSceneCircumscribingRectangle();
            bool matching = false;

            if (EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-8, -5)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(2, 7)) ||
                EqualScenePoints(sceneRectangle.Vertex1, new ScenePoint(-8, 7)) &&
                EqualScenePoints(sceneRectangle.Vertex2, new ScenePoint(2, -5)))
            {
                matching = true;
            }

            Assert.That(matching,
               Is.True,
               "Координаты прямоугольника, описывающего сцену, неверные");
        }

        private bool EqualScenePoints(ScenePoint scenePoint1, ScenePoint scenePoint2)
        {
            double Eps = 1e-10;

            return (Math.Abs(scenePoint1.X - scenePoint2.X) < Eps && Math.Abs(scenePoint1.Y - scenePoint2.Y) < Eps) ? true : false;
        }
    }
}
