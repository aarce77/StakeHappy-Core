using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class SearchFixture : TestBase
    {
        [Fact]
        public void Successful()
        {
            // data
            var invoiceId1 = new Guid();
            var invoiceId2 = new Guid();
            var criteria = new Core.Data.Search.InvoiceCriteria() { UserId = Guid.NewGuid() };

            var invoices = new List<Core.Data.Model.Invoice>
            {
                new Core.Data.Model.Invoice {Id = invoiceId1},
                new Core.Data.Model.Invoice {Id = invoiceId2}
            };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor, null);

            bll.Expect(b => b.Search(criteria)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            bll.Expect(b => b.VaildateCriteria(criteria));
            invoicePersistor.Expect(d => d.Search(criteria)).Return(invoices.AsQueryable());

            // record
            Mocks.ReplayAll();

            var results = bll.Search(criteria);

            Assert.NotEmpty(results);
            Assert.NotNull(results.FirstOrDefault(s => s.Id == invoiceId1));
            Assert.NotNull(results.FirstOrDefault(s => s.Id == invoiceId2));
        }
    }
}