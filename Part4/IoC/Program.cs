using System;

namespace IoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WeirdInteger[] array = new WeirdInteger[] {
                new WeirdInteger(-1),
                new WeirdInteger(1),
                new WeirdInteger(2),
                new WeirdInteger(-2),
                new WeirdInteger(-3),
                new WeirdInteger(3),
                new WeirdInteger(0),
            };

            System.Array.Sort(array);

            foreach (WeirdInteger value in array)
                System.Console.WriteLine(value);

            Console.ReadLine();
        }
    }

    class WeirdInteger : System.IComparable<WeirdInteger>
    {
        private int value;

        public WeirdInteger(int value)
        {
            this.value = value;
        }

        public int CompareTo(WeirdInteger other)
        {
            if (value == other.value)
                return 0;
            else if (value < 0 && other.value >= 0)
                return 1;
            else if (value >= 0 && other.value < 0)
                return -1;
            else
                return value.CompareTo(other.value);
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
