namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Figures;
    using Scene2d.Exceptions;
    using System;

    class AddCircleCommandBuilder : ICommandBuilder
    {
        const string name = @"(\d|\w|-|_){1,}";
        const string point = @"\(-?\d{1,},\s?-?\d{1,}\)";
        private static readonly Regex RecognizeRegex = new Regex(name + @"\s" + point + @" radius -?\d{1,}$");
        private IFigure _circle;
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
                line = match.ToString();
                _name = Regex.Match(line, name).ToString().Trim();
                line = line.Remove(0, _name.Length).Trim();

                var coordinateMatch = Regex.Match(line, point);
                var coordinate = new double[2];
                var i = 0;

                foreach (var valueCoordinate in Regex.Matches(coordinateMatch.ToString(), @"-?\d{1,}"))
                {
                    if (valueCoordinate.ToString().Length > 1 && valueCoordinate.ToString()[0] == '0')
                    {
                        throw new BadFormatException();
                    }

                    coordinate[i] = double.Parse(valueCoordinate.ToString());
                    i++;
                }

                line = line.Remove(0, coordinateMatch.ToString().Length);

                var radius = double.Parse(Regex.Match(line, @"-?\d{1,}").ToString());

                if (radius <= 0)
                {
                    throw new BadCircleRadius(); 
                }

                var p = new ScenePoint { X = coordinate[0], Y = coordinate[1] };

                _circle = new CircleFigure(p, radius);
            }
            else
            {
                throw new BadFormatException();
            }
        }

        public ICommand GetCommand() => new AddFigureCommand(_name, _circle);

    }
}
