using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class SaveInvoiceItemFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var item = new Core.Data.Model.InvoiceItem
            {
                Id = Guid.NewGuid()
            };
            var ex = Assert.Throws<ArgumentException>(() => 
                new Core.Logic.InvoiceLogic().SaveInvoiceItem(item));
            Assert.Equal("invoice id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            var item = new Core.Data.Model.InvoiceItem
            {
                Id = id,
                Invoice_Id = Guid.NewGuid()
            };

            // mocks
            var invoiceItemPersistor = Mocks.StrictMock<Core.Data.Persistor.InvoiceItem>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(null, invoiceItemPersistor, null);

            bll.Expect(b => b.SaveInvoiceItem(item)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoiceItemPersistor.Expect(d => d.Save(item));
            invoiceItemPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.SaveInvoiceItem(item);
        }
    }
}