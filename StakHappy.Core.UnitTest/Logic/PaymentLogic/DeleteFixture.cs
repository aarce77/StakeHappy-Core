using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.PaymentLogic
{
    [Collection("Logic")]
    public class DeleteFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.PaymentLogic().Delete(Guid.Empty));
            Assert.Equal("Payment id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();

            // mocks
            var paymentPersistor = Mocks.StrictMock<Core.Data.Persistor.Payment>();
            var bll = Mocks.StrictMock<Core.Logic.PaymentLogic>(paymentPersistor);

            bll.Expect(b => b.Delete(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            paymentPersistor.Expect(d => d.Delete(id));
            paymentPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.Delete(id);
        }
    }
}