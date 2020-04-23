namespace QuadraticEqRoot2
{
    using System.Collections.Generic;
    using System;

    public static class Solver
    {
        public static IList<object> Solve(double a, double b, double c)
        {

            const double Eps = 0.000000001;

            object x1 = null;
            object x2 = null;

            if (Math.Abs(a) > Eps)
            {
                if (Math.Abs(b) > Eps)
                {
                    var d = Math.Pow(b, 2) - (4 * a * c);

                    if (d > 0)
                    {
                        x1 = (-b + Math.Sqrt(d)) / (2 * a);
                        x2 = (-b - Math.Sqrt(d)) / (2 * a);
                    }

                    if (Math.Abs(d) < Eps)
                    {
                        x1 = -b / (2 * a);
                    }
                }
                else {

                    var quotient = - c / a;

                    if (quotient > 0) {
                        x1 = Math.Sqrt(-c / a);
                        x2 = -Math.Sqrt(-c / a);
                    }
                }

            }
            else {

                if (Math.Abs(b) > Eps)
                {
                    x1 = -c / b;
                }
            }

            return new[] { x1, x2 };
        }
    }
}