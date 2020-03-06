using System;
using NumbersToWords.WordConverter;

namespace NumbersToWords
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter a Number to convert to words");
            var number = Console.ReadLine() ?? "0";

            number = decimal.Parse(number).ToWords();

            Console.WriteLine("Number in words is \n{0}", number);
            Console.ReadKey();
        }
    }
}
