namespace Scene2d.Tests
{
    using NUnit.Framework;
    using Scene2d.CommandBuilders;
    using Scene2d.Exceptions;

    [TestFixture]
    class CommandBuildersTests
    {
        [Test]
        public void СommandProducer_AddWrongCommand_AppendLineShouldThrowException()//Adding the wrong command
        {
            var commandProducer = new CommandProducer();

            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add ellipse"),
                "Неправильная команда принялась за правильную");
            /* Assert.That(() => commandProducer.AppendLine("add ellipse"),
                 Throws.TypeOf<BadFormatException> (), 
                 "Неправильная команда принялась за правильную");*/
        }

        [Test]
        public void CommandBuilder_AddRectangleWrongCommand_ShouldThrowException()//Adding the wrong command for rectangle 
        {
            var commandProducer = new CommandProducer();

            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add rectangle R1 (-10, 02) (10, 20)"),
                "Команда с координатой, начинающейся с нуля, принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add rectangle R1"),
               "Команда без координат принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add rectangle R1 (-102) (10, 20)"),
                "Команда с неверно заданными координатами принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add rectangle R1 (-10, -20)"),
                "Команда, имеющая только координаты начала, принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add rectangle (-10, -20) (10, 20)"),
                "Команда без имени прямоугольника принялась за правильную");
        }

        [Test]
        public void CommandBuilder_AddCircleWrongCommand_ShouldThrowException()//Adding the wrong command for circle
        {
            var commandProducer = new CommandProducer();

            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add circle C1 (-10, 02) radius 10"),
                "Команда с координатой, начинающейся с нуля, принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add circle C1 (-102) radius 10"),
                "Команда с неверно заданной координатой принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add circle C1 radius 10"),
                "Команда без координат центра принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add circle C1 (-10, 2)"),
               "Команда без радиуса принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add circle (-10, 2) radius 10"),
               "Команда без имени окружности принялась за правильную");

            commandProducer.currentBuilder = null;
            Assert.Throws<BadCircleRadius>(() => commandProducer.AppendLine("add circle C1 (-10, 2) radius -10"),
             "Команда с отрицательным радиусом принялась за правильную");
        }

        [Test]
        public void CommandBuilder_AddPolygonWrongCommand_ShouldThrowException()//Adding the wrong command for polygon
        {
            var commandProducer = new CommandProducer();

            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add polygon"),
                "Команда без имени принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add point (-102)"),
                "Команда с неверно заданной координатой принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            Assert.Throws<BadFormatException>(() => commandProducer.AppendLine("add point"),
                "Команда без координат принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (10,2)");
            Assert.Throws<BadPolygonPoint>(() => commandProducer.AppendLine("add point (10,2)"),
                "Команда c уже имеющейся в полигоне точкой принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-3,1)");
            commandProducer.AppendLine("add point (1,5)");
            Assert.Throws<BadPolygonPoint>(() => commandProducer.AppendLine("add point (-1,3)"),
                "Команда c точкой, которая образует пересечение сегментов, принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P2");
            commandProducer.AppendLine("add point (-3,1)");
            commandProducer.AppendLine("add point (1,5)");
            commandProducer.AppendLine("add point (3,2)");
            Assert.Throws<BadPolygonPoint>(() => commandProducer.AppendLine("add point (-4,4)"),
                "Команда2 c точкой, которая образует пересечение сегментов, принялась за правильную");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-1100, -300)");
            commandProducer.AppendLine("add point (-700, 200)");
            Assert.Throws<BadPolygonPointNumber>(() => commandProducer.AppendLine("end polygon"),
                "Полигон с двумя точками принялся за правильный");

            commandProducer.currentBuilder = null;
            commandProducer.AppendLine("add polygon P1");
            commandProducer.AppendLine("add point (-1100, -300)");
            commandProducer.AppendLine("add point (-700, 200)");
            Assert.That(() => commandProducer.IsCommandReady,
                Throws.TypeOf<UnexpectedEndOfPolygon>(),
                "Добавление частичного полигона принялось за правильное");
        }

        [Test]
        public void CommandBuilder_AddFigureWhoseNameAlreadyExists_ShouldThrowException()//Adding figure whose name already exists
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("add rectangle R1 (-4, -3) (2, 6)");
            var command = commandProducer.GetCommand();
            command.Apply(scene);

            commandProducer.AppendLine("add rectangle R1 (-5, 3) (4, 0)");
            command = commandProducer.GetCommand();

            Assert.Throws<NameDoesAlreadyExist>(() => command.Apply(scene),
            "Произошло добавление фигуры, имя которой уже есть в списке фигур в сцене");
        }

        [Test]
        public void CommandBuilder__ShouldThrowException()//Move figure whose name does not exist in the list of scene figures
        {
            var commandProducer = new CommandProducer();
            var scene = new Scene();

            commandProducer.AppendLine("move R1 (10, 2)");
            var command = commandProducer.GetCommand();

            Assert.Throws<BadName>(() => command.Apply(scene),
            "Произошло перемещение фигуры, которой не существует в списке фигур сцены");
        }
    }
}
