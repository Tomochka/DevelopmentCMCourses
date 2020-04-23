namespace Interpolation
{
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            double[] values = { 1, 8, -4, 5, 2, -5, 4, 2, 0, 1, 5 };

            Console.Write("Data array: ");
            foreach (var value in values) Console.Write(value + " ");
            Console.WriteLine();

            Console.Write("Enter a point: ");
            var samplePoint = Convert.ToDouble(Console.ReadLine());

            CommonInterpolator[] interpolators =
                {
                    new StepInterpolator(values), new LinearInterpolator(values), new NewtonInterpolator(values), new СubicInterpolator(values)
                };

            Console.WriteLine("Calculating value at sample point: {0}", samplePoint);

            foreach (var interpolator in interpolators)
            {
                Console.WriteLine("Class {0}: Interpolated value is {1:F4}", interpolator.GetType().Name, interpolator.CalculateValue(samplePoint));
            }
        }
    }
}
