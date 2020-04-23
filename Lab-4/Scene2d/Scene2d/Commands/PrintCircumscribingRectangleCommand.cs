namespace Scene2d.Commands
{
    using System;

    class PrintCircumscribingRectangleCommand : ICommand
    {
        private readonly string _name;

        public PrintCircumscribingRectangleCommand(string name)
        {
            _name = name;
        }

        public void Apply(Scene scene)
        {
            if (_name == "scene")
            {
                SceneRectangle ScPoint = scene.CalculateSceneCircumscribingRectangle();

                Console.WriteLine("Сoordinates сircumscribing rectangle scene" +
                   " (" + ScPoint.Vertex1.X + ", " + ScPoint.Vertex1.Y + ") " +
                   " (" + ScPoint.Vertex2.X + ", " + ScPoint.Vertex2.Y + ")");
            }
            else {

                SceneRectangle ScPoint = scene.CalculateCircumscribingRectangle(_name);

                Console.WriteLine("Сoordinates сircumscribing rectangle " + _name + 
                   " (" + ScPoint.Vertex1.X + ", " + ScPoint.Vertex1.Y + ") " +
                   " (" + ScPoint.Vertex2.X + ", " + ScPoint.Vertex2.Y + ")");
            }
        }

        public string FriendlyResultMessage
        {
            get { return "Displayed coordinates " + _name; }
        }
    }
}
