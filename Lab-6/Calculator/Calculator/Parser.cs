namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Exceptions;

    public class Parser : IParser
    {
       
        public Operation Parse(string inputString)
        {
            string[] partsOperation = inputString.Trim().Split(' ');
            string sign;
            List<double> parameters = new List<double>();

            if (partsOperation[0].First() >= '0' && partsOperation[0].First() <= '9') throw new NoSignOfOperationException();
            
            sign = partsOperation[0];

            for (var i = 1; i < partsOperation.Length; i++)
            {
                if (partsOperation[i] != String.Empty)
                {
                    if (double.TryParse(partsOperation[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double parameter))
                    {
                        parameters.Add(parameter);
                    }
                    else 
                    {
                        throw new InvalidParameterInOperationException();
                    }
                }
            }

            if (parameters.Count == 0) throw new IncorrectParametersException();
            // Формат строки: {имя_операции} {параметр1} ... {параметрN}
            
            // Обратите внимание на юнит-тесты для этого класса
            return new Operation(sign, parameters.ToArray()) ;
        }
    }
}

