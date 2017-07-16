using System;
using System.Collections.Generic;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class SaveFixture : TestBase
    {
        [Fact]
        public void UserIdEmpty()
        {
            var client = new Core.Data.Model.Client();

            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.ClientLogic().Save(client));

            Assert.Equal("User id most be specified to save a client", ex.Message);
        }

        [Fact]
        public void SuccessfulUpdate()
        {
            // data
            var client = new Core.Data.Model.Client
            {
                Id = Guid.NewGuid(),
                User_Id = Guid.NewGuid(),
                Active = true,
                CreatedDate = DateTime.Now,
                Contacts = new List<Core.Data.Model.ClientContact>
                {
                    new Core.Data.Model.ClientContact
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            // mocks
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(clientPersistor, null);

            // record
            bll.Expect(b => b.Save(client)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientPersistor.Expect(u => u.Save(client)).Return(client);
            clientPersistor.Expect(u => u.Commit()).Return(1);

            Mocks.ReplayAll();
            var result = bll.Save(client);

            Assert.Equal(client.Id, result.Id);
        }

        [Fact]
        public void SuccessfulInsert()
        {
            // data
            var client = new Core.Data.Model.Client
            {
                Id = Guid.Empty,
                User_Id = Guid.NewGuid(),
                Active = true,
                CreatedDate = DateTime.Now,
                Contacts = new List<Core.Data.Model.ClientContact>
                {
                    new Core.Data.Model.ClientContact
                    {
                        Id = Guid.NewGuid()
                    },
                    new Core.Data.Model.ClientContact
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            // mocks
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var clientContactPersistor = Mocks.StrictMock<Core.Data.Persistor.ClientContact>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(clientPersistor, clientContactPersistor);

            // record
            bll.Expect(b => b.Save(client)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientPersistor.Expect(u => u.Save(client)).Return(client).WhenCalled(c => client.Id = Guid.NewGuid());
            clientContactPersistor.Expect(
                c => c.Save(Arg<Core.Data.Model.ClientContact>.Matches(m => m.Client_Id == client.Id && m.Client_Id != Guid.Empty)))
                .Repeat.Twice();

            clientPersistor.Expect(u => u.Commit()).Return(1);
            clientContactPersistor.Expect(u => u.Commit()).Return(1);

            Mocks.ReplayAll();
            var result = bll.Save(client);

            Assert.Equal(client.Id, result.Id);
        }
    }
}
