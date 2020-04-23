namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Exceptions;

    class MoveCommandBuilder: ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        const string point = @"\(-?\d{1,},\s?-?\d{1,}\)";

        private static readonly Regex RecognizeRegex = new Regex(@"^move");
        private string _name;
        private ScenePoint _vector;

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

            var matchPoint = Regex.Match(line, point);
            
            if (matchPoint.Success)
            {
                var coordinate = new double[2];
                var i = 0;

                foreach (var valueCoordinate in Regex.Matches(matchPoint.ToString(), @"-?\d{1,}"))
                {
                    if (valueCoordinate.ToString().Length > 1 && valueCoordinate.ToString()[0] == '0')
                    {
                        throw new BadFormatException();
                    }

                    coordinate[i] = double.Parse(valueCoordinate.ToString());
                    i++;
                }

                _vector.X = coordinate[0];
                _vector.Y = coordinate[1];
            }
        }

        public ICommand GetCommand() => new MoveCommand(_name, _vector);
    }
}
