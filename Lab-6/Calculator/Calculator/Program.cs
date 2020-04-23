namespace Calculator
{
    using System;

    using Exceptions;

    public static class Program
    {
        public static void Main()
        {
            ICalculatorEngine calculator = new CalculatorEngine();
            IParser parser = new Parser();

            try
            {
                // ++
                calculator.DefineOperation("++", a => a + 1);

                // +
                calculator.DefineOperation("+", (a, b) => a + b);
                calculator.DefineOperation("+", (a, b, c) => a + b + c);
               
                // --
                calculator.DefineOperation("--", a => a - 1);

                // -
                calculator.DefineOperation("-", a => -a);
                calculator.DefineOperation("-", (a, b) => a - b);
                calculator.DefineOperation("-", (a, b, c) => a - b - c);

                //*
                calculator.DefineOperation("*", (x, y) => x * y);

                // /
                var division = new Func<double, double, double>(
                   (x, y) =>
                   {
                       if (y == 0)
                       {
                           throw new ArgumentOutOfRangeException();
                       }

                       return x / y;
                   });

                calculator.DefineOperation("/", division);

                // pow 
                calculator.DefineOperation("^", Math.Pow);

                // sqrt
                var sqrt = new Func<double, double>(
                    x =>
                    {
                        if (x < 0)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        return Math.Sqrt(x);
                    });

                calculator.DefineOperation("sqrt", sqrt);

                // mod
                var mod = new Func<double,double, double>(
                    (x, y) =>
                    {
                        if (y == 0)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        return x % y;
                    });

                calculator.DefineOperation("mod", mod); 

                // abs
                calculator.DefineOperation("abs", Math.Abs);

                // min
                calculator.DefineOperation("min", Math.Min);
                calculator.DefineOperation("min", (x, y, z) => Math.Min(x, Math.Min(y,z)));

                //max
                calculator.DefineOperation("max", Math.Max);
                calculator.DefineOperation("max", (x, y, z) => Math.Max(x, Math.Max(y, z)));

                //exp
                calculator.DefineOperation("exp", Math.Exp);

                //log
                var log1 = new Func<double, double>(
                   x =>
                   {
                       if (x <= 0)
                       {
                           throw new ArgumentOutOfRangeException();
                       }

                       return Math.Log(x);
                   });

                calculator.DefineOperation("log", log1);

                var log2 = new Func<double, double, double>(
                   (x, y) =>
                   {
                       if (x <= 0 || y <= 0 || y == 1 )
                       {
                           throw new ArgumentOutOfRangeException();
                       }

                       return Math.Log(x, y);
                   });

                calculator.DefineOperation("log", log2);

                //log10
                var log10 = new Func<double, double>(
                 x =>
                 {
                     if (x <= 0)
                     {
                         throw new ArgumentOutOfRangeException();
                     }

                     return Math.Log10(x);
                 });
                
                calculator.DefineOperation("log10", log10);
            }
            catch (AlreadyExistsOperationException)
            {
                Console.WriteLine("This operation already exists in the calculator");
            }

            var evaluator = new Evaluator(calculator, parser);
            Console.WriteLine("Please enter expressions, enter 'exit' to exit: \n");
            string line = Console.ReadLine();

            while (line != "exit")
            {
                if (line == null || line.Trim().Length == 0)
                {
                    break;
                }

                try
                {
                    Console.WriteLine("= " + evaluator.Calculate(line) + "\n");

                    Console.WriteLine("Please enter expressions, enter 'exit' to exit: \n");
                    line = Console.ReadLine();
                }
                catch (NotFoundOperationException)
                {
                    Console.WriteLine("This operation was not found in the calculator");
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("The operation cannot be performed with these parameters");
                    break;
                }
                catch (ParametersCountMismatchException)
                {
                    Console.WriteLine("Invalid number of parameters for this operation");
                    break;
                }
                catch (IncorrectParametersException)
                {
                    Console.WriteLine("There are no parameters in the entered string");
                    break;
                }
                catch (NoSignOfOperationException)
                {
                    Console.WriteLine("There is no sign of the operation");
                    break;
                }
                catch (InvalidParameterInOperationException)
                {
                    Console.WriteLine("The operation contains invalid parameters");
                    break;
                }

            }
        }
    }
}
