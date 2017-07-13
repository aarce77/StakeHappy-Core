using Xunit;

namespace StakHappy.Core.UnitTest.Data.Model.Currency.Currency
{
    public class SetCurrencyFixture
    {
        [Fact]
        public void GreaterThanOneDollar()
        {
            var currency = new Core.Data.Model.Currency.Currency(23456);
            Assert.Equal(234.56, currency.Value);
        }

        [Fact]
        public void OneDollar()
        {
            var currency = new Core.Data.Model.Currency.Currency(100);
            Assert.Equal(1, currency.Value);
        }

        [Fact]
        public void LessThanOneDollar()
        {
            var currency = new Core.Data.Model.Currency.Currency(50);
            Assert.Equal(.5, currency.Value);
        }

        [Fact]
        public void OneCent()
        {
            var currency = new Core.Data.Model.Currency.Currency(1);
            Assert.Equal(.01, currency.Value);
        }

        [Fact]
        public void Zero()
        {
            var currency = new Core.Data.Model.Currency.Currency(0);
            Assert.Equal(0, currency.Value);
        }

        [Fact]
        public void Negative()
        {
            var currency = new Core.Data.Model.Currency.Currency(-20);
            Assert.Equal(-0.2, currency.Value);
        }
    }
}
