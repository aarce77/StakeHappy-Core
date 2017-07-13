using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Client
{
    [Collection("Data")]
    public class SearchFixture : PersistorBase
    {
        private readonly Guid _userId;
        private readonly Guid _badClientId;
        private readonly Dictionary<Guid, string> _clients; 

        #region Constructor
        public SearchFixture()
        {
            var userId = Guid.NewGuid();
            _userId = Guid.NewGuid();
            _badClientId = Guid.NewGuid();
            _clients = new Dictionary<Guid, string>
            {
                {Guid.Parse("{14D83039-606E-4B67-8A43-62743A3A6BEC}"), "A"},
                {Guid.Parse("{006D3BA4-81A2-4FDB-96F9-4E86351454D1}"), "B"},
                {Guid.Parse("{FB642139-0640-4359-B94E-A5A948E78E89}"), "C"},
                {Guid.Parse("{7D0CF51F-3386-47B8-8CBF-51173CD5AA0D}"), "D"}
            };

            var repo = new Core.Data.Persistor.Client();

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(_userId, "test-username", "tester@tester.cc"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(userId, "test-username2", "tester2@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                    Utils.Client.GetInsertScript(_badClientId, userId, "non-retrieved-company"));

            foreach (var client in _clients)
            {
                repo.DbContext.Database.ExecuteSqlCommand(
                    Utils.Client.GetInsertScript(client.Key, _userId, client.Value));
            }
        }
        #endregion

        [Fact]
        public void ByUser()
        {
            var repo = new Core.Data.Persistor.Client();
            var coll = repo.Search(new Core.Data.Search.ClientCriteria {UserId = _userId}).ToList();

            Assert.Equal(4, coll.Count());
            foreach (var clientId in _clients.Select(client => client.Key))
            {
                var id = clientId;
                Assert.NotNull(coll.FirstOrDefault(r => r.Id == id));
            }
        }

        [Fact]
        public void ByUserAndCompanyName()
        {
            var repo = new Core.Data.Persistor.Client();
            var coll = repo.Search(new Core.Data.Search.ClientCriteria { UserId = _userId, CompanyName = "Company_C" }).ToList();

            Assert.Equal(1, coll.Count());
            Assert.NotNull(coll.FirstOrDefault(r => r.Id == Guid.Parse("FB642139-0640-4359-B94E-A5A948E78E89")));
        }

        [Fact]
        public void ByPageAndPageSize()
        {
            var repo = new Core.Data.Persistor.Client();
            var coll = repo.Search(new Core.Data.Search.ClientCriteria { UserId = _userId, Page = 1, PageSize = 2 }).ToList();

            Assert.Equal(2, coll.Count());
            Assert.Null(coll.FirstOrDefault(c => c.Id == _badClientId));
        }
    }
}