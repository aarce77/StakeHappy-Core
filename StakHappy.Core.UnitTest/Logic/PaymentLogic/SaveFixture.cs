using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.PaymentLogic
{
    [Collection("Logic")]
    public class SaveFixture : TestBase
    {
        [Fact]
        public void InvoiceIdEmpty()
        {
            var payment = new Core.Data.Model.Payment();

            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.PaymentLogic().Save(payment));

            Assert.Equal("Invoice id most be specified to apply a payment", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            var payment = new Core.Data.Model.Payment()
            {
                Invoice_Id = Guid.NewGuid()
            };

            // mocks
            var paymentPersistor = Mocks.StrictMock<Core.Data.Persistor.Payment>();
            var bll = Mocks.StrictMock<Core.Logic.PaymentLogic>(paymentPersistor);
            
            // record
            bll.Expect(b => b.Save(payment)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            paymentPersistor.Expect(u => u.Save(payment)).Return(payment);
            paymentPersistor.Expect(u => u.Commit()).Return(1);

            Mocks.ReplayAll();
            var result = bll.Save(payment);

            Assert.Equal(payment.Id, result.Id);

            Mocks.VerifyAll();
        }
    }
}