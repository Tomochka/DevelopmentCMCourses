namespace Interpolation
{
    using System;
    internal class StepInterpolator : CommonInterpolator
    {
        public StepInterpolator(double[] values) : base(values)
        {
            // Nothing to do
        }

        public override double CalculateValue(double x)
        {
            var nmax = Values.Length - 1;
            return (nmax < 0) ? base.CalculateValue(x) :
                Values[Math.Max(0, Math.Min((int)Math.Round(x), nmax))];
        }
    }
}
