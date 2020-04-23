namespace Scene2d.Commands
{
    class ReflectCommand: ICommand
    {
        private readonly string _name;
        private readonly ReflectOrientation _orientation;

        public ReflectCommand(string name, ReflectOrientation orientation)
        {
            _name = name;
            _orientation = orientation;
        }

        public void Apply(Scene scene)
        {
            if (_name == "scene") scene.ReflectScene(_orientation);
            else scene.Reflect(_name,_orientation);
        }

        public string FriendlyResultMessage
        {
            get { return "Reflect " + _name + " " + _orientation.ToString(); }
        }
    }
}
