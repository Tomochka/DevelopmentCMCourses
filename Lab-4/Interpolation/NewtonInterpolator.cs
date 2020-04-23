namespace Interpolation
{
    internal class NewtonInterpolator : CommonInterpolator
    {
        public NewtonInterpolator(double[] values) : base(values)
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

            //Newton's interpolation formulas
            var y = new double[Values.Length, Values.Length]; //Finite difference table

            for (var i = 0; i < Values.Length; i++)
            {
                y[i, 0] = Values[i];
            }

            var k = 0;

            for (var j = 1; j < Values.Length; j++)
            {
                k++;

                for (var i = 0; i < Values.Length - k; i++)
                {
                    y[i, j] = y[i + 1, j - 1] - y[i, j - 1];
                }
            }

            var average = Values.Length / 2;
            var factorial = 1;
            var forCalcFactorial = 2;

            if (x <= average)
            {
                //First interpolation formula of Newton
                var t = x;
                var pn = y[0, 0];
                var nextT = t;

                for (var i = 1; i < Values.Length; i++)
                {
                    pn += (nextT * y[0, i]) / factorial;
                    factorial *= forCalcFactorial;
                    forCalcFactorial++;
                    t--;
                    nextT *= t;
                }

                return pn;
            }
            else
            {
                //Second interpolation formula of Newton
                var t = x - (Values.Length - 1);
                var pn = y[Values.Length - 1, 0];
                var nextT = t;

                for (var j = 1; j < Values.Length; j++)
                {
                    pn += (nextT * y[Values.Length - 1 - j, j]) / factorial;
                    factorial *= forCalcFactorial;
                    forCalcFactorial++;
                    t++;
                    nextT *= t;
                }

                return pn;
            }
        }
    }
}
