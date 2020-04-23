namespace Scene2d.Commands
{
    class MoveCommand: ICommand
    {
        private readonly string _name;
        private readonly ScenePoint _vector;

        public MoveCommand(string name, ScenePoint vector)
        {
            _name = name;
            _vector = vector;
        }

        public void Apply(Scene scene)
        {
            if (_name == "scene") scene.MoveScene(_vector);
            else scene.Move(_name, _vector);
        }

        public string FriendlyResultMessage
        {
            get { return "Move " + _name + " on vector (" + _vector.X + ", " + _vector.Y + ")";}
        }
    }
}
