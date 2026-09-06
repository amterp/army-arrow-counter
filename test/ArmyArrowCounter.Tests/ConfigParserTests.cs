using System.Xml;
using Xunit;

namespace ArmyArrowCounter.Tests {
    public class ConfigParserTests {
        [Fact]
        public void ReadsBothSettings() {
            ParsedConfig config = ConfigParser.Parse(
                "<Config><CounterType>EXACT_PERCENT</CounterType><Prefix>Ammo: </Prefix></Config>");

            Assert.Equal(CounterType.EXACT_PERCENT, config.CounterType);
            Assert.Equal("Ammo: ", config.Prefix);
            Assert.Empty(config.Warnings);
        }

        [Fact]
        public void KeepsTrailingSpaceInThePrefix() {
            // The shipped config relies on this; without it the counter reads "Army arrows:12 / 40".
            Assert.Equal("Army arrows: ",
                ConfigParser.Parse("<Config><Prefix>Army arrows: </Prefix></Config>").Prefix);
        }

        [Fact]
        public void AllowsAnEmptyPrefix() {
            ParsedConfig config = ConfigParser.Parse("<Config><Prefix></Prefix></Config>");
            Assert.Equal("", config.Prefix);
        }

        [Fact]
        public void FallsBackAndWarnsOnAnUnknownCounterType() {
            ParsedConfig config = ConfigParser.Parse(
                "<Config><CounterType>SPARKLES</CounterType><Prefix>x</Prefix></Config>");

            Assert.Equal(ConfigParser.DEFAULT_COUNTER_TYPE, config.CounterType);
            string warning = Assert.Single(config.Warnings);
            Assert.Contains("SPARKLES", warning);
        }

        [Fact]
        public void FallsBackAndWarnsWhenCounterTypeIsAbsent() {
            ParsedConfig config = ConfigParser.Parse("<Config><Prefix>x</Prefix></Config>");

            Assert.Equal(ConfigParser.DEFAULT_COUNTER_TYPE, config.CounterType);
            Assert.Contains("CounterType", Assert.Single(config.Warnings));
        }

        [Fact]
        public void FallsBackAndWarnsWhenPrefixIsAbsent() {
            ParsedConfig config = ConfigParser.Parse("<Config><CounterType>EXACT_PERCENT</CounterType></Config>");

            Assert.Equal(ConfigParser.DEFAULT_PREFIX, config.Prefix);
            Assert.Contains("Prefix", Assert.Single(config.Warnings));
        }

        [Fact]
        public void WarnsAboutBothWhenTheFileIsEmptyOfSettings() {
            Assert.Equal(2, ConfigParser.Parse("<Config></Config>").Warnings.Count);
        }

        [Fact]
        public void IgnoresTagsItDoesNotKnow() {
            ParsedConfig config = ConfigParser.Parse(
                "<Config><CounterType>EXACT_PERCENT</CounterType><Prefix>x</Prefix><FutureSetting>7</FutureSetting></Config>");

            Assert.Equal(CounterType.EXACT_PERCENT, config.CounterType);
            Assert.Empty(config.Warnings);
        }

        [Fact]
        public void ThrowsOnMalformedXml() {
            Assert.Throws<XmlException>(() => ConfigParser.Parse("<Config><Prefix>oops</Config>"));
        }

        [Fact]
        public void RoundTripsEveryCounterType() {
            foreach (CounterType counterType in System.Enum.GetValues(typeof(CounterType))) {
                ParsedConfig config = ConfigParser.Parse(
                    $"<Config><CounterType>{counterType}</CounterType><Prefix>x</Prefix></Config>");
                Assert.Equal(counterType, config.CounterType);
            }
        }
    }
}
