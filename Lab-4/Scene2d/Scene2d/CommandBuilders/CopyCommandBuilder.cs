namespace Scene2d.CommandBuilders
{
    using Scene2d.Commands;
    using System.Text.RegularExpressions;

    class CopyCommandBuilder: ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        private static readonly Regex RecognizeRegex = new Regex(@"^copy");
        private string _copyName;
        private string _originalName;

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
                line = line.Remove(match.Index, match.Length).Trim();
            }

            var matchName = Regex.Matches(line, name);
            var copyName = false;

            foreach (var name in matchName)
            {
                var nameStr = name.ToString().Trim();

                if (nameStr == "to") copyName = true;
                else
                {
                    if (!copyName) _originalName = nameStr;
                    else _copyName = nameStr;
                }
            };
        }

        public ICommand GetCommand() => new CopyCommand(_originalName, _copyName);
    }
}

