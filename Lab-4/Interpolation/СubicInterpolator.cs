namespace Interpolation
{
    using System;
    internal class СubicInterpolator : CommonInterpolator
    {
        public СubicInterpolator(double[] values) : base(values)
        {
        }

        public override double CalculateValue(double x)
        {
            if (Values.Length < 1)
            {
                return base.CalculateValue(x);
            }

            if (x < 0)
            {
                return Values[0];
            }

            if ((int)x >= Values.Length - 1)
            {
                return Values[Values.Length - 1];
            }

            //Sweep method
            var a = new double[Values.Length - 2];
            var b = new double[Values.Length - 2];
            var c = new double[Values.Length - 2];
            var d = new double[Values.Length - 2];

            for (var i = 1; i < Values.Length - 2; i++)
            {
                a[i] = 1;
            }

            a[0] = 0; //Condition

            for (var i = 0; i < Values.Length - 2; i++)
            {
                b[i] = -2 * 2;
            }

            for (var i = 0; i < Values.Length - 2; i++)
            {
                c[i] = 1;
            }

            c[Values.Length - 3] = 0; //Condition

            for (var i = 0; i < Values.Length - 2; i++)
            {
                d[i] = 6 * (Values[i + 2] - Values[i + 1] - (Values[i + 1] - Values[i]));
            }

            //For forward motion
            var alfa = new double[Values.Length - 1];
            var betta = new double[Values.Length - 1];
            alfa[0] = 0;
            betta[0] = 0;

            //Forward motion
            for (var i = 0; i < Values.Length - 2; i++)
            {
                alfa[i + 1] = c[i] / (b[i] - (a[i] * alfa[i]));
                betta[i + 1] = ((a[i] * betta[i]) - d[i]) / (b[i] - (a[i] * alfa[i]));
            }

            //Reverse motion
            c = new double[Values.Length];
            c[Values.Length - 1] = 0;
            c[0] = 0;

            for (var i = Values.Length - 2; i >= 1; i--)
            {
                c[i] = (alfa[i] * c[i + 1]) + betta[i];
            }

            a = new double[Values.Length];
            b = new double[Values.Length];
            d = new double[Values.Length];

            for (var i = 1; i < Values.Length; i++)
            {
                a[i] = Values[i];
            }

            for (var i = 1; i < Values.Length; i++)
            {
                d[i] = c[i] - c[i - 1];
            }

            for (var i = 1; i < Values.Length; i++)
            {
                b[i] = (0.5 * c[i]) - ((Math.Pow(1, 2) * d[i]) / 6) + (Values[i] - Values[i - 1]);
            }

            //Cubic spline
            double s = 0;

            for (var i = 1; i < Values.Length; i++)
            {
                if ((x >= i - 1) && (x <= i))
                {
                    s = a[i] + (b[i] * (x - i)) + ((c[i] / 2) * Math.Pow(x - i, 2)) + ((d[i] / 6) * Math.Pow(x - i, 3));
                }
            }

            return s;
        }
    }
}
