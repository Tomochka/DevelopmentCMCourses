namespace Interpolation
{
    internal class CommonInterpolator
    {
        protected double[] Values;

        public CommonInterpolator(double[] array)
        {
            Values = array;
        }

        public virtual double CalculateValue(double x)
        {
            return 0;
        }
    }
}
