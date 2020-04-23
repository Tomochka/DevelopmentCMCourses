namespace Scene2d.Commands
{
    class CopyCommand : ICommand
    {
        private readonly string _name;
        private readonly string _name2;

        public CopyCommand(string name, string name2)
        {
            _name = name;
            _name2 = name2;
        }
        public void Apply(Scene scene)
        {
            if (_name == "scene") scene.CopyScene(_name2);
            else scene.Copy(_name,_name2);
        }

        public string FriendlyResultMessage
        {
            get
            {
                return "Сopied " + _name + ". Name clone is " + _name2;
            }
        }
    }
}
