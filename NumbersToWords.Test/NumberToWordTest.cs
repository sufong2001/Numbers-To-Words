using NumbersToWords.OtherImplementation;
using NumbersToWords.WordConverter;
using Nut;
using Xunit;
using Xunit.Abstractions;

namespace NumbersToWords.Test
{
    public class NumberToWordTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NumberToWordTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Testing the Nut library output
        /// </summary>
        [Fact]
        public void NutToWordsTest()
        {
            var number = 123456.21m;
            var words = number.ToText(Nut.Currency.USD, Nut.Language.English);

            _testOutputHelper.WriteLine(words);

            Assert.Equal("one hundred twenty three thousand four hundred fifty six dollars twenty one cents", words);
        }

        /// <summary>
        /// Testing other implementation output
        /// </summary>
        [Fact]
        public void ToWordsV1Test()
        {
            var number = 123456.21m;
            var words = number.ToWordsV1();

            _testOutputHelper.WriteLine(words);

            Assert.Equal("One hundred twenty-three thousand, four hundred fifty-six and 21/100", words);
        }

        [Fact]
        public void N123ToWordTest()
        {
            int number = 123;

            var words = new EnglishNumber().WordsInHundred(number);

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("One Hundred Twenty-Three", words);
        }

        [Fact]
        public void N123678ToWordTest()
        {
            int number = 123678;

            var words = new EnglishNumber().WordsInScale(number);

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("One Hundred Twenty-Three Thousand Six Hundred Seventy-Eight", words);
        }

        [Fact]
        public void MaxLongToWordTest()
        {
            var number = long.MaxValue;

            var words = new decimal(number).ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal(
                "Nine Quintillion Two Hundred Twenty-Three Quadrillion Three Hundred Seventy-Two Trillion Thirty-Six Billion Eight Hundred Fifty-Four Million Seven Hundred Seventy-Five Thousand Eight Hundred Seven Dollars"
                , words);
        }

        [Fact]
        public void DollarAndCentToWordTest()
        {
            var number = 1200147483647.01m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal(
                "One Trillion Two Hundred Billion One Hundred Forty-Seven Million Four Hundred Eighty-Three Thousand Six Hundred Forty-Seven Dollars and One Cent"
                , words);
        }

        [Fact]
        public void N12_3456ToWordTest()
        {
            var number = 12.3456m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal(
                "Twelve Dollars and Thirty-Five Cents"
                , words);
        }


        [Fact]
        public void N0ToWordTest()
        {
            var number = 0m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("Zero Dollar", words);
        }

        [Fact]
        public void N1ToWordTest()
        {
            var number = 1m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("One Dollar", words);
        }

        [Fact]
        public void N0_01ToWordTest()
        {
            var number = 0.01m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("One Cent", words);
        }

        [Fact]
        public void N0_10ToWordTest()
        {
            var number = 0.10m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("Ten Cents", words);
        }

        [Fact]
        public void N1_15ToWordTest()
        {
            var number = 1.15m;

            var words = number.ToWords();

            _testOutputHelper.WriteLine($"{number} => {words}");

            Assert.Equal("One Dollar and Fifteen Cents", words);
        }

        /// <summary>
        /// Test the number between 0 and 999 to compare with other implementation online
        /// </summary>
        [Fact]
        public void WordInHundredRangeTest()
        {
            var number = 0;

            do
            {
                var words = new EnglishNumber().WordsInHundred(number)
                    .Replace("-", " ")
                    .ToLower();

                var wordsv1 = new decimal(number).ToWordsV1()
                    .Replace(" and 00/100", "")
                    .Replace("2", " ")
                    .Replace("-", " ")
                    .ToLower();

                var nut = ((long)number).ToText();

                if (words != wordsv1 || words != nut)
                {
                    _testOutputHelper.WriteLine($"{number} => {words} => {wordsv1} => {nut}");
                }

                Assert.Equal(wordsv1, words);
                Assert.Equal(nut, words);
            } while (++number < 1000);
        }

        /// <summary>
        /// Test the number between 1000 and 1000000000 to compare with other implementation online
        /// </summary>
        [Fact]
        public void WordInThousandRangeTest()
        {
            var number = 1000m;

            do
            {
                var words = number.ToWords()
                    .Replace(" Dollars", "")
                    .Replace(" Dollar", "")
                    .Replace("-", " ")
                    .ToLower();

                var wordsv1 = number.ToWordsV1()
                    .Replace(" and 00/100", "")
                    .Replace("2", " ")
                    .Replace("-", " ")
                    .Replace(",", "")
                    .ToLower();

                var nut = ((long)number).ToText();

                if (words != wordsv1 || words != nut)
                {
                    _testOutputHelper.WriteLine($"{number} => {words} => {wordsv1} => {nut}");
                }

                Assert.Equal(wordsv1, words);
                Assert.Equal(nut, words);

                number += 1000;
            } while (number++ <= 1000000000);
        }
    }
}