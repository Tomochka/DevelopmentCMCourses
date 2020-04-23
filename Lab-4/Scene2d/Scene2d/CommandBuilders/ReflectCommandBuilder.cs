namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;

    class ReflectCommandBuilder: ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        private static readonly Regex RecognizeRegex = new Regex(@"^reflect (vertically|horizontally)");
        private ReflectOrientation _orientation;
        private string _name;

        public bool IsCommandReady
        {
            get
            {
                return true;
            }
        }

        public void AppendLine(string line)
        {
            var match = RecognizeRegex.Match(line);

            if (match.Success)
            {
                var matchOrientation = Regex.Match(line, @"(vertically|horizontally)");

                if (matchOrientation.Success)
                {
                    if (matchOrientation.Value == "vertically") _orientation = ReflectOrientation.Vertical;
                    else _orientation = ReflectOrientation.Horizontal;
                }

                _name = line.Remove(match.Index, match.Length).Trim();
            }  
        }

        public ICommand GetCommand() => new ReflectCommand(_name, _orientation);
    }
}
