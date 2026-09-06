using System.Globalization;
using Xunit;

namespace ArmyArrowCounter.Tests {
    public class ExactFractionFormatterTests {
        [Theory]
        [InlineData(3, 10, "", "3 / 10")]
        [InlineData(0, 0, "", "0 / 0")]
        [InlineData(3, 10, "Army arrows: ", "Army arrows: 3 / 10")]
        public void Formats(int remaining, int max, string prefix, string expected) {
            Assert.Equal(expected, new ExactFractionFormatter().Format(remaining, max, prefix));
        }
    }

    public class ExactPercentFormatterTests {
        [Theory]
        [InlineData(1, 2, "50%")]
        [InlineData(1, 3, "33%")]
        [InlineData(2, 3, "67%")]
        [InlineData(10, 10, "100%")]
        [InlineData(0, 10, "0%")]
        [InlineData(0, 0, "0%")]
        public void Formats(int remaining, int max, string expected) {
            Assert.Equal(expected, new ExactPercentFormatter().Format(remaining, max, ""));
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("fr-FR")]
        [InlineData("de-DE")]
        [InlineData("")]
        public void RendersIdenticallyInEveryCulture(string culture) {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            Assert.Equal("50%", new ExactPercentFormatter().Format(1, 2, ""));
        }
    }

    public class NearestXPercentFormatterTests {
        [Theory]
        // Exact midpoints round up, consistently. Before the fix these alternated,
        // because Math.Round defaults to banker's rounding: 5% became ~0%.
        [InlineData(10, 5, 100, "~10%")]
        [InlineData(10, 15, 100, "~20%")]
        [InlineData(10, 25, 100, "~30%")]
        [InlineData(10, 35, 100, "~40%")]
        [InlineData(10, 44, 100, "~40%")]
        [InlineData(20, 30, 100, "~40%")]
        [InlineData(25, 13, 100, "~25%")]
        [InlineData(10, 0, 0, "~0%")]
        [InlineData(10, 10, 10, "~100%")]
        public void Formats(int roundTo, int remaining, int max, string expected) {
            Assert.Equal(expected, new NearestXPercentFormatter(roundTo).Format(remaining, max, ""));
        }
    }

    public class NearestWrittenFormatterTests {
        // Each threshold tested at the boundary and just below it. The boundaries only
        // land in the intended band because the proportion is computed in double: as a
        // float, 71/100 widened to 0.70999997, one band low.
        [Theory]
        [InlineData(10, 10, "All")]
        [InlineData(875, 1000, "Almost all")]
        [InlineData(874, 1000, "About three quarters")]
        [InlineData(71, 100, "About three quarters")]
        [InlineData(70, 100, "About two thirds")]
        [InlineData(585, 1000, "About two thirds")]
        [InlineData(584, 1000, "About half")]
        [InlineData(415, 1000, "About half")]
        [InlineData(414, 1000, "About one third")]
        [InlineData(29, 100, "About one third")]
        [InlineData(28, 100, "About one quarter")]
        [InlineData(125, 1000, "About one quarter")]
        [InlineData(124, 1000, "Almost no")]
        [InlineData(1, 1000, "Almost no")]
        [InlineData(0, 10, "No")]
        [InlineData(0, 0, "No")]
        public void DescribesAmount(int remaining, int max, string expectedDescription) {
            Assert.Equal(expectedDescription + " ammunition remaining.",
                new NearestWrittenFormatter().Format(remaining, max, ""));
        }

        [Fact]
        public void LowercasesTheSentenceWhenAPrefixPrecedesIt() {
            Assert.Equal("Army arrows: all ammunition remaining.",
                new NearestWrittenFormatter().Format(10, 10, "Army arrows: "));
        }

        [Fact]
        public void CapitalizesTheSentenceWhenItStartsTheLine() {
            Assert.Equal("All ammunition remaining.",
                new NearestWrittenFormatter().Format(10, 10, ""));
        }
    }

    public class ArrowTextFormattersTests {
        [Theory]
        [InlineData(CounterType.EXACT_FRACTION, "1 / 2")]
        [InlineData(CounterType.EXACT_PERCENT, "50%")]
        [InlineData(CounterType.NEAREST_10_PERCENT, "~50%")]
        [InlineData(CounterType.NEAREST_20_PERCENT, "~60%")]
        [InlineData(CounterType.NEAREST_25_PERCENT, "~50%")]
        [InlineData(CounterType.NEAREST_WRITTEN, "About half ammunition remaining.")]
        public void MapsEveryCounterTypeToAFormatter(CounterType counterType, string expected) {
            Assert.Equal(expected, ArrowTextFormatters.For(counterType).Format(1, 2, ""));
        }
    }
}
