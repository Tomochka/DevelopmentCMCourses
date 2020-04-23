namespace QuadraticEqRoot
{
    using System;

    public class Program
    {
        public static void Main()
        {
            const double Eps = 0.000001;

            Console.WriteLine("Please set the quadratic equation");

            Console.Write("a: ");
            var a = Convert.ToDouble(Console.ReadLine());

            Console.Write("b: ");
            var b = Convert.ToDouble(Console.ReadLine());

            Console.Write("c: ");
            var c = Convert.ToDouble(Console.ReadLine());

            var d = Math.Pow(b, 2) - (4 * a * c);


            if (Math.Abs(a) > Eps || (Math.Abs(a) > Eps && Math.Abs(b) > Eps))
            {
                if (d > 0)
                {
                    var x1 = (-b + Math.Sqrt(d)) / (2 * a);
                    var x2 = (-b - Math.Sqrt(d)) / (2 * a);
                    Console.WriteLine("The root is {0}", x1);
                    Console.WriteLine("The root is {0}", x2);
                }

                if (Math.Abs(d) < Eps)
                {
                    var x = -b / (2 * a);
                    Console.WriteLine("The root is {0}", x);
                }

                if (d < 0)
                {
                    Console.WriteLine("This quadratic equation has no real roots");
                }
            }
            else
            {
                Console.WriteLine("This equation is not square");
            }
        }
    }
}
