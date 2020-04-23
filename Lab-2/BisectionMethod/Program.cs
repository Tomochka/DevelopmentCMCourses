namespace BisectionMethod
{
    using System;
    public class Program
    {
        public static double Function(double x)
        {
            return (x * x) - 2;
        }

        public static void Main()
        {
            var a = 0.0;
            var b = 2.0;
            double y;
            const double eps = 0.0001;
            var i = 0; // Number of iterations  

            ////Bisection Method
            if (Function(a) * Function(b) >= 0)
            {
                y = Function(a) <= eps ? b : a;
            }
            else
            {
                do
                {
                    i++;
                    y = (b + a) / 2;

                    if (Math.Abs(Function(y)) <= eps)
                    {
                        break;
                    }
                    else
                    {
                        if (Function(a) * Function(y) < 0)
                        {
                            b = y;
                        }
                        else
                        {
                            a = y;
                        }
                    }
                }
                while (Math.Abs(b - a) >= eps);
            }

            Console.WriteLine("Корень уравнения равен " + Math.Round(y, 5));
            Console.WriteLine("Количество итераций равно " + i);
        }
    }
}
