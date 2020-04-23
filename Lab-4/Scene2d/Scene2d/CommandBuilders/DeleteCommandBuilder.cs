namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;

    class DeleteCommandBuilder : ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        private static readonly Regex RecognizeRegex = new Regex(@"^delete ");
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
                line = line.Remove(match.Index, match.Length).Trim();
            }

            var matchName = Regex.Match(line, name);

            if (matchName.Success)
            {
                _name = matchName.Value;
            }
        }

        public ICommand GetCommand() => new DeleteCommand(_name);
    }
}
