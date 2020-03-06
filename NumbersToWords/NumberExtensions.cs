using System;
using System.Collections.Generic;
using System.Text;
using NumbersToWords.WordConverter;

namespace NumbersToWords
{
    public static class NumberExtensions
    {
        public static string ToWords(this decimal number)
        {
            return new EnglishNumber().ToWords(number);
        }
    }
}
