using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NumbersToWords.WordConverter
{
    public class EnglishNumber
    {
        internal Dictionary<int, string> NumberRange;
        private static Dictionary<int, string> ScaleRange;

        public EnglishNumber()
        {
            NumberRange = new Dictionary<int, string>()
            {
                {0, "Zero"},
                {1, "One"},
                {2, "Two"},
                {3, "Three"},
                {4, "Four"},
                {5, "Five"},
                {6, "Six"},
                {7, "Seven"},
                {8, "Eight"},
                {9, "Nine"},
                {10, "Ten"},
                {11, "Eleven"},
                {12, "Twelve"},
                {13, "Thirteen"},
                {14, "Fourteen"},
                {15, "Fifteen"},
                {16, "Sixteen"},
                {17, "Seventeen"},
                {18, "Eighteen"},
                {19, "Nineteen"},
                {20, "Twenty"},
                {30, "Thirty"},
                {40, "Forty"},
                {50, "Fifty"},
                {60, "Sixty"},
                {70, "Seventy"},
                {80, "Eighty"},
                {90, "Ninety"}
            };

            ScaleRange = new Dictionary<int, string>()
            {
                {0, ""},
                {1, ""}, // ten
                {2, "Hundred"},
                {3, "Thousand"},
                {6, "Million"},
                {9, "Billion"},
                {12, "Trillion"},
                {15, "Quadrillion"},
                {18, "Quintillion"},
                {21, "Sextillion"},
            };
        }

        /// <summary>
        /// Handle the number less than thousand to words
        /// </summary>
        /// <param name="number"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public virtual string WordsInHundred(int number, int scale = 2)
        {
            var words = "";

            if (NumberRange.ContainsKey(number)) return NumberRange[number];

            var s = (int)Math.Pow(10, scale);
            var hundred = number / s;
            var ten = 0;
            var teen = number - hundred * s;

            if (hundred > 0)
            {
                words += $"{NumberRange[hundred]} {ScaleRange[2]}";
            }

            if (teen > 20)
            {
                ten = (teen / 10) * 10;
                teen -= ten;

                words += $" {NumberRange[ten]}";
            }

            if (teen > 0)
            {
                words += (ten > 0 ? "-" : " ") + $"{NumberRange[teen]}";
            }

            return words.Trim();
        }

        /// <summary>
        /// Handle the number in different scales and start with Sextillion (scale 21)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public virtual string WordsInScale(long number, int scale = 21, int minScale = 3)
        {
            var s = (long)Math.Pow(10, scale);
            var val = (int)(number / s);
            var nextNumber = number - val * s;

            var words = val > 0
                ? $"{WordsInHundred(val)} {ScaleRange[scale]}"
                : "";

            var nextScale = scale - minScale; // 9 -3
            if (nextScale >= 0 && nextNumber > 0)
            {
                // recursive to the next scale
                var nextWords = WordsInScale(nextNumber, nextScale);

                words = $"{words} {nextWords}";
            }

            return words.Trim();
        }

        /// <summary>
        /// Determine the number is more than one than it will return 's'
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string Plural(long number)
        {
            return number > 1 ? "s" : "";
        }

        /// <summary>
        /// A general function to convert a number to word.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string ToWords(decimal value)
        {
            var numbers = value.ToString("F", CultureInfo.InvariantCulture).Split('.');

            var bigger = long.Parse(numbers.First());
            var smaller = long.Parse(numbers.Skip(1).FirstOrDefault() ?? "0");

            var val = WordsInScale(bigger);

            if (bigger > 0)
            {
                val = $"{val} Dollar{Plural(bigger)}";
            }

            if (bigger > 0 && smaller > 0)
            {
                val += " and ";
            }

            if (smaller > 0)
            {
                var words = WordsInScale(smaller);
                val += $"{words} Cent{Plural(smaller)}".Trim();
            }

            if (value == 0.0m)
            {
                val = $"{NumberRange[(int)value]} Dollar";
            }


            return val.Trim();
        }
    }
}