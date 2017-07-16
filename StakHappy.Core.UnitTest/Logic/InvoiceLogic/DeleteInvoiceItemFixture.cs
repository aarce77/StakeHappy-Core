using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class DeleteInvoiceItemFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Core.Logic.InvoiceLogic().DeleteInvoiceItem(Guid.Empty));
            Assert.Equal("invoice item id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();

            // mocks
            var invoiceItemPersistor = Mocks.StrictMock<Core.Data.Persistor.InvoiceItem>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(null, invoiceItemPersistor);

            bll.Expect(b => b.DeleteInvoiceItem(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoiceItemPersistor.Expect(d => d.Delete(id));
            invoiceItemPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.DeleteInvoiceItem(id);
        } 
    }
}