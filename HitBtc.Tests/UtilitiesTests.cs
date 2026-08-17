using System;
using System.Globalization;
using Xunit;

namespace Hitbtc.Tests
{
    public class UtilitiesTests
    {
        [Theory]
        [InlineData("Hello", "hello")]
        [InlineData("A", "a")]
        [InlineData("alreadyLower", "alreadyLower")]
        public void FirstCharToLower_ValidInput_LowersOnlyFirstCharacter(string input, string expected)
        {
            Assert.Equal(expected, Utilities.FirstCharToLower(input));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FirstCharToLower_EmptyInput_ThrowsArgumentException(string input)
        {
            Assert.Throws<ArgumentException>(() => Utilities.FirstCharToLower(input));
        }

        [Fact]
        public void FirstCharToLower_TurkishCulture_UsesInvariantCasing()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
                Assert.Equal("id", Utilities.FirstCharToLower("Id"));
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }
    }
}
