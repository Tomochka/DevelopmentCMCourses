namespace QuadraticEqRoot2
{
    using System.Collections.Generic;
    using System.IO;
    using System;
    using System.Globalization;

    class Program
    {
        static void Main()
        {
            const string outputPath = "output.txt";

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            using (var input = File.OpenText("input.txt"))
            using (var output = File.CreateText(outputPath))
            {
                string line;

                while ((line = input.ReadLine()) != null)
                {
                    string[] coefficients = line.Trim().Split(",");

                    
                    double a = double.Parse(coefficients[0], CultureInfo.InvariantCulture);
                    double b = double.Parse(coefficients[1], CultureInfo.InvariantCulture);
                    double c = double.Parse(coefficients[2], CultureInfo.InvariantCulture);

                    IList<object> roots = Solver.Solve(a, b, c);
                                        
                    var outputLine = String.Empty;

                    if (roots[1] == null)
                    {
                        if (roots[0] == null)
                        {
                            outputLine = "This quadratic equation has no real roots";
                        }
                        else
                        {
                            outputLine = ((double)roots[0]).ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        outputLine = $"{((double)roots[0]).ToString(CultureInfo.InvariantCulture)}, " +
                            $"{((double)roots[1]).ToString(CultureInfo.InvariantCulture)}";
                    }
                    
                    output.WriteLine(outputLine);
                }
            }
        }
    }
}
