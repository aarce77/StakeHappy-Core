using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class GetFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.InvoiceLogic().Get(Guid.Empty));

            Assert.Equal("invoice id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            var user = new Core.Data.Model.Invoice { Id = id };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor, null);

            bll.Expect(b => b.Get(user.Id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoicePersistor.Expect(d => d.Get(user.Id)).Return(user);

            // record
            Mocks.ReplayAll();

            var result = bll.Get(id);

            Assert.Equal(id, result.Id);
        }
    }
}
