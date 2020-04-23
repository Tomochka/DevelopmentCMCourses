namespace Scene2d.Commands
{
    using System;
    class DeleteCommand: ICommand
    {
        private readonly string _name;

        public DeleteCommand(string name)
        {
            _name = name;
        }

        public void Apply(Scene scene)
        {
            Console.WriteLine(_name);
            if (_name == "scene") scene.DeleteScene();
            else scene.Delete(_name);
        }

        public string FriendlyResultMessage
        {
            get { return "Deleted " + _name; }
        }
    }
}
