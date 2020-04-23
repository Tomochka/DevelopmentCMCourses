namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Exceptions;
    using System.Collections.Generic;

    class GroupFiguresCommandBuilder : ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";

        private static readonly Regex RecognizeRegexGroup = new Regex(@"^group");
        private string _groupName;
        private List<string> _names = new List<string>();

        public bool IsCommandReady
        {
            get
            {
                return true;
            }
        }

        public void AppendLine(string line)
        {
            var matchGroup = RecognizeRegexGroup.Match(line);
            
            if (matchGroup.Success)         
            {
                line = line.Remove(matchGroup.Index, matchGroup.Length).Trim();
            }

            var matchName = Regex.Matches(line, name);
            var groupName = false;

            foreach (var name in matchName)
            {
                var nameStr = name.ToString().Trim();

                if (nameStr == "as") groupName = true;    
                else {

                    if (!groupName) _names.Add(nameStr);
                    else _groupName = nameStr;
                }  
            };
        }

        public ICommand GetCommand() => new GroupCommand(_names, _groupName);

    }
}
