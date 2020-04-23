namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class CalculatorEngine : ICalculatorEngine
    {
        private Dictionary<string, Func<double, double>> UnaryOperations = new Dictionary<string, Func<double, double>>(); //??
        private Dictionary<string, Func<double, double, double>> BinaryOperations = new Dictionary<string, Func<double, double, double>>(); //??
        private Dictionary<string, Func<double, double, double, double>> TernaryOperations = new Dictionary<string, Func<double, double, double, double>>(); //??

        public double PerformOperation(Operation operation)
        {
            var operationSign = operation.Sign;
            var operationParam = operation.Parameters;

            // Обратите внимание на юнит-тесты для этого класса
            switch (operationParam.Length)
            {
                case 1:
                    if (UnaryOperations.ContainsKey(operationSign))
                    {
                        UnaryOperations.TryGetValue(operationSign, out Func<double, double> func);

                        return func(operationParam[0]);
                    }
                    else
                    {
                        if (BinaryOperations.ContainsKey(operationSign) || TernaryOperations.ContainsKey(operationSign))
                        {
                            throw new ParametersCountMismatchException();
                        }
                        else
                        {
                            throw new NotFoundOperationException();
                        }
                    }
                case 2:
                    if (BinaryOperations.ContainsKey(operationSign))
                    {
                        BinaryOperations.TryGetValue(operationSign, out Func<double, double, double> func);

                        return func(operationParam[0], operationParam[1]);
                    }
                    else 
                    {
                        if (UnaryOperations.ContainsKey(operationSign) || TernaryOperations.ContainsKey(operationSign))
                        {
                            throw new ParametersCountMismatchException();
                        }
                        else
                        {
                            throw new NotFoundOperationException();
                        }
                    }

                case 3:
                    if (TernaryOperations.ContainsKey(operationSign))
                    {
                        TernaryOperations.TryGetValue(operationSign, out Func<double, double, double, double> func);

                        return func(operationParam[0], operationParam[1], operationParam[2]);
                    }
                    else
                    {
                        if (UnaryOperations.ContainsKey(operationSign) || BinaryOperations.ContainsKey(operationSign))
                        {
                            throw new ParametersCountMismatchException();
                        }
                        else
                        {
                            throw new NotFoundOperationException();
                        }
                    }

                default:
                    {
                        if (!UnaryOperations.ContainsKey(operationSign) && !BinaryOperations.ContainsKey(operationSign)
                            && !TernaryOperations.ContainsKey(operationSign))
                        {
                            throw new NotFoundOperationException();
                        }
                        else
                        {
                            throw new ParametersCountMismatchException();
                        }
                    }
                    
            }
        }


        // Обратите внимание на юнит-тесты для этого класса 

        public void DefineOperation(string sign, Func<double, double> body)
        {
            if (UnaryOperations.ContainsKey(sign)) 
            {
                throw new AlreadyExistsOperationException();
            }

            UnaryOperations.Add(sign, body);
        }

        public void DefineOperation(string sign, Func<double, double, double> body)
        {
            if (BinaryOperations.ContainsKey(sign))
            {
                throw new AlreadyExistsOperationException();
            }

            BinaryOperations.Add(sign, body);
        }

        public void DefineOperation(string sign, Func<double, double, double, double> body)
        {
            if (TernaryOperations.ContainsKey(sign))
            {
                throw new AlreadyExistsOperationException();
            }

            TernaryOperations.Add(sign, body);
        }
    }
}
