using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class DeleteContactFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Core.Logic.ClientLogic().DeleteContact(Guid.Empty));
            Assert.Equal("client contact id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();

            // mocks
            var clientContactPersistor = Mocks.StrictMock<Core.Data.Persistor.ClientContact>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(null, clientContactPersistor);

            bll.Expect(b => b.DeleteContact(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientContactPersistor.Expect(d => d.Delete(id));
            clientContactPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.DeleteContact(id);
        }
    }
}
