using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Client
{
    [Collection("Data")]
    public class SaveFixture : PersistorBase
    {
        [Fact]
        public void Successful()
        {
            var persistor = new Core.Data.Persistor.Client();
            var userId = Guid.Parse("{5CEAD906-4825-4357-A60D-F0363B247CA6}");

            var sql = Utils.User.GetInsertScript(userId, "user 1", "test@tester.com");

            persistor.DbContext.Database.ExecuteSqlCommand(sql);
            
            var clientId = Guid.Empty;
            var client = new Core.Data.Model.Client
            {
                Id = clientId,
                Active = true,
                CompanyName = "Client name",
                CreatedDate = DateTime.Now,
                User_Id = userId,
            };

            persistor.Save(client);
            persistor.Commit();

            const string sqlFormatter = "IF NOT EXISTS (" +
                                        "SELECT COUNT(*) FROM Clients WHERE Id = '{0}' AND CompanyName = '{1}' AND User_Id = '{2}' " +
                                        "HAVING COUNT(*) = 1" +
                                        ") RAISERROR ('Error creating client.',16,1);";

            Assert.NotEqual(clientId, client.Id);

            sql = string.Format(sqlFormatter, client.Id, "Client name", userId);
            persistor.DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}