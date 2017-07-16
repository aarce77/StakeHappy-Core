using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.PaymentLogic
{
    [Collection("Logic")]
    public class GetFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.PaymentLogic().Get(Guid.Empty));

            Assert.Equal("Payment id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            var payment = new Core.Data.Model.Payment() { Id = id };

            // mocks
            var paymentPersistor = Mocks.StrictMock<Core.Data.Persistor.Payment>();
            var bll = Mocks.StrictMock<Core.Logic.PaymentLogic>(paymentPersistor);

            bll.Expect(b => b.Get(payment.Id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            paymentPersistor.Expect(d => d.Get(payment.Id)).Return(payment);

            // record
            Mocks.ReplayAll();

            var result = bll.Get(id);

            Assert.Equal(id, result.Id);
        }
    }
}