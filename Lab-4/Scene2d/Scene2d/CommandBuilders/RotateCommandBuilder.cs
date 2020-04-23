namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;

    class RotateCommandBuilder: ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        private static readonly Regex RecognizeRegex = new Regex(@"^rotate");
        private double _angle;
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
                line = line.Remove(matchName.Index, matchName.Length).Trim();
            }

            var matchAngle = Regex.Match(line, @"\d{1,}");

            if (matchAngle.Success)
            {
                _angle = double.Parse(matchAngle.ToString());
            }
      
        }

        public ICommand GetCommand() => new RotateCommand(_name, _angle);
    }
}
