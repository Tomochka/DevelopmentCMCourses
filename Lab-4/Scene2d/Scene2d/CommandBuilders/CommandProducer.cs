namespace Scene2d.CommandBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Scene2d.Commands;
    using Scene2d.Exceptions;

    public class CommandProducer : ICommandBuilder
    {
        const string name = @"\s(\d|\w|-|_){1,}";
        const string point = @"\(-?\d{1,},\s?-?\d{1,}\)";

        private static readonly Dictionary<Regex, Func<ICommandBuilder>> Commands =
            new Dictionary<Regex, Func<ICommandBuilder>>
            {
                { new Regex(@"^add rectangle" + name + @"\s" + point + @"\s" + point + @"$"), () => new AddRectangleCommandBuilder() },
                { new Regex(@"^add circle" + name + @"\s" + point + @" radius -?\d{1,}$"), () => new AddCircleCommandBuilder() },
                { new Regex(@"^add polygon"+ name + @"$"), () => new AddPolygonCommandBuilder() },
                { new Regex(@"^group(,?\s(\d|\w|-|_){1,}){2,} as" + name + @"$"), () => new GroupFiguresCommandBuilder() },
                { new Regex(@"^delete" + name + @"$"), () => new DeleteCommandBuilder() },
                { new Regex(@"^copy" + name + " to" + name + @"$"), () => new CopyCommandBuilder() },
                { new Regex(@"^move" + name + @"\s" + point + @"$"), () => new MoveCommandBuilder() },
                { new Regex(@"^rotate" + name + @" \d{1,}$"), () => new RotateCommandBuilder() },
                { new Regex(@"^reflect (vertically|horizontally)" + name + @"$"), () => new ReflectCommandBuilder() },
                { new Regex(@"^print circumscribing rectangle for" + name + @"$"), () => new PrintCommandBuilder() },
            };

        private ICommandBuilder _currentBuilder;

        public ICommandBuilder currentBuilder 
        {
            set { _currentBuilder = value; }
        }

        public bool IsCommandReady
        {
            get
            {
                if (_currentBuilder == null)
                {
                    return false;
                }

                return _currentBuilder.IsCommandReady;
            }
        }

        public void AppendLine(string line)
        {
            if (_currentBuilder == null)
            {
                foreach (var pair in Commands)
                {
                    if (pair.Key.IsMatch(line))
                    {
                        _currentBuilder = pair.Value();
                        break;
                    }
                }

                if (_currentBuilder == null)
                {
                    throw new BadFormatException();
                }
            }

            _currentBuilder.AppendLine(line);
        }

        public ICommand GetCommand()
        {

            if (_currentBuilder == null)
            {
                return null;
            }

            var command = _currentBuilder.GetCommand();
            _currentBuilder = null;

            return command;
        }
    }
}
