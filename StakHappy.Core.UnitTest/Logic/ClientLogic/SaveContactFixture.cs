using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class SaveContactFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var contact = new Core.Data.Model.ClientContact
            {
                Id = Guid.NewGuid()
            };
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.ClientLogic().SaveContact(contact));
            Assert.Equal("client id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            var contact = new Core.Data.Model.ClientContact
            {
                Id = id,
                Client_Id = Guid.NewGuid()
            };

            // mocks
            var clientContactPersistor = Mocks.StrictMock<Core.Data.Persistor.ClientContact>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(null, clientContactPersistor);
            

            bll.Expect(b => b.SaveContact(contact)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientContactPersistor.Expect(d => d.Save(contact));
            clientContactPersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.SaveContact(contact);
        }
    }
}
