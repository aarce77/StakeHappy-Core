using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace StakHappy.Core.UnitTest.Logic.ClientLogic
{
    [Collection("Logic")]
    public class SearchFixture : TestBase
    {
        [Fact]
        public void CriteriaNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => 
                new Core.Logic.ClientLogic().Search(null));

            Assert.Equal("critiera", ex.ParamName);
        }

        [Fact]
        public void UserIdEmpty()
        {
            var criteria = new Core.Data.Search.ClientCriteria();

            var ex = Assert.Throws<ArgumentNullException>(() => new Core.Logic.ClientLogic().Search(criteria));

            Assert.Equal("UserId", ex.ParamName);
        }

        [Fact]
        public void Successful()
        {
            // data
            var clientId1 = new Guid();
            var clientId2 = new Guid();
            var criteria = new Core.Data.Search.ClientCriteria {UserId = Guid.NewGuid()};

            var clients = new List<Core.Data.Model.Client>
            {
                new Core.Data.Model.Client {Id = clientId1},
                new Core.Data.Model.Client {Id = clientId2}
            };

            // mocks
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var bll = Mocks.StrictMock<Core.Logic.ClientLogic>(clientPersistor, null);

            bll.Expect(b => b.Search(criteria)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            bll.Expect(b => b.VaildateCriteria(criteria)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            clientPersistor.Expect(d => d.Search(criteria)).Return(clients.AsQueryable());

            // record
            Mocks.ReplayAll();

            var results = bll.Search(criteria);

            Assert.NotEmpty(results);
            Assert.NotNull(results.FirstOrDefault(s => s.Id == clientId1));
            Assert.NotNull(results.FirstOrDefault(s => s.Id == clientId2));
        }
    }
}
