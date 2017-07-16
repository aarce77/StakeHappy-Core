using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class DeleteFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.ClientLogic().Delete(Guid.Empty));
            Assert.Equal("client id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var clientId = Guid.NewGuid();

            // mocks
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(clientPersistor, null);

            bll.Expect(b => b.Delete(clientId)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientPersistor.Expect(d => d.Delete(clientId));
            clientPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.Delete(clientId);
        }
    }
}
