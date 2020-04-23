namespace Scene2d.Commands
{
    class RotateCommand : ICommand 
    {
        private readonly string _name;
        private readonly double _angle;

        public RotateCommand(string name, double angle)
        {
            _name = name;
            _angle = angle;
        }

        public void Apply(Scene scene)
        {
            if (_name == "scene") scene.RotateScene(_angle);
            else scene.Rotate(_name, _angle);
        }

        public string FriendlyResultMessage
        {
            get { return "Rotate " + _name + " on angle " + _angle; }
        }
    }
}
