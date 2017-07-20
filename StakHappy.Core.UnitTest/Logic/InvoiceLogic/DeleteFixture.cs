using System;
using System.Collections.Generic;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class DeleteFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Core.Logic.InvoiceLogic().Delete(Guid.Empty));
            Assert.Equal("invoice id cannot be empty", ex.Message);
        }

        [Fact]
        public void SuccessfulWithoutPayments()
        {
            // data
            var id = Guid.NewGuid();
            var invoice = new Core.Data.Model.Invoice {Id = id};

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor, null, null);

            bll.Expect(b => b.Delete(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoicePersistor.Expect(d => d.Get(id)).Return(invoice);
            invoicePersistor.Expect(d => d.Delete(id));
            invoicePersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.Delete(id);
        }

        [Fact]
        public void FailureWithPayments()
        {
            // data
            var id = Guid.NewGuid();
            var invoice = new Core.Data.Model.Invoice
            {
                Id = id,
                Payments = new List<Core.Data.Model.Payment> { new Core.Data.Model.Payment { Id = Guid.NewGuid() } }
            };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor,null, null);

            bll.Expect(b => b.Delete(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoicePersistor.Expect(d => d.Get(id)).Return(invoice);

            // record
            Mocks.ReplayAll();

            var ex = Assert.Throws<NotImplementedException>(() => bll.Delete(id));
            Assert.Equal("an invoice that contains a payment can not be deleted", ex.Message);
        }
    }
}