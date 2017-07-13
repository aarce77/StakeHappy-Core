using Xunit;

namespace StakHappy.Core.UnitTest.Data.Model.InvoiceItem
{
    public class CurrencySubTotalFixture
    {
        [Fact]
        public void Successful()
        {
            var item = new Core.Data.Model.InvoiceItem {UnitCost = 2995, Quantity = 2};
            
            Assert.Equal(59.90, item.CurrencySubTotal.Value);
        }
    }
}
