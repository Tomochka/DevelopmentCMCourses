namespace Scene2d.Commands
{
    using System.Collections.Generic;

    class GroupCommand: ICommand
    {
        private List<string> _namesList = new List<string>();
        private string _groupName;

        public GroupCommand(List<string> names, string groupName)
        {
            for (var i = 0; i < names.Count; i++) {
                _namesList.Add(names[i]); 
            }

            _groupName = groupName;
        }

        public void Apply(Scene scene)
        {
            scene.CreateCompositeFigure(_groupName, _namesList);
        }

        public string FriendlyResultMessage
        {
            get
            {
                var names = "";

                for (var i = 0; i < _namesList.Count; i++)
                {
                    names += _namesList[i] + ", ";
                }

                names = names.Remove(names.Length - 2, 2);
                
                return "Elements with names " + names + " are grouped. Group name " + _groupName + ".";

            }
        }

    }
}

