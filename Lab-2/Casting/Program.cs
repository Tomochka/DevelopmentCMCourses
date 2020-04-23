namespace Casting
{
    using System;
    public class Program
    {
        public enum Weekday : byte
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday = 16,
            Sunday
        }

        [Flags]
        public enum BookAttributes : short
        {
            IsNothing = 0x0,
            IsEducational = 0x1,
            IsDetective = 0x2,
            IsHumoros = 0x4,
            IsMedical = 0x8,
            IsPolitical = 0x10,
            IsEconomical = 0x20
        }

        public static void Main()
        {
            Console.Write("Enter an integer: ");
            var a = Convert.ToInt32(Console.ReadLine());
            var b = (Weekday) a;
            var c = (BookAttributes) a;

            if (b.GetType() == Weekday.Monday.GetType())
            {
                Console.WriteLine("Type Weekday: {0}", b);
            }

            if (c.GetType() == BookAttributes.IsNothing.GetType())
            {
                Console.WriteLine("Type BookAttributes: {0}", c);
            }
        }
    }
}
