using System;
using Xunit;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class GetFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.ClientLogic().Get(Guid.Empty));
            
            Assert.Equal("id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var clientId = Guid.NewGuid();
            var client = new Core.Data.Model.Client {Id = clientId};

            // mocks
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(clientPersistor, null);

            bll.Expect(b => b.Get(client.Id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientPersistor.Expect(d => d.Get(client.Id)).Return(client);

            // record
            Mocks.ReplayAll();

            var result = bll.Get(clientId);

            Assert.Equal(clientId, result.Id);
        }
    }
}
