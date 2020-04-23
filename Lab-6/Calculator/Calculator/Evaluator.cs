namespace Calculator
{
    using System.Globalization;

    public class Evaluator
    {
        private readonly ICalculatorEngine _calculatorEngine;

        private readonly IParser _parser;

        public Evaluator(ICalculatorEngine calculatorEngine, IParser parser)
        {
            _calculatorEngine = calculatorEngine;
            _parser = parser;
        }

        public string Calculate(string inputString)
        {
            // Обратите внимание на юнит-тесты для этого класса
            return (_calculatorEngine.PerformOperation(_parser.Parse(inputString))).ToString();
        }
    }
}
