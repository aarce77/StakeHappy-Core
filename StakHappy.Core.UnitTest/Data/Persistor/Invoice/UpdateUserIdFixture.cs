using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Invoice
{
    [Collection("Data")]
    public class UpdateUserIdFixture : PersistorBase
    {
        [Fact]
        public void Successful()
        {
            var userId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var clientId = Guid.NewGuid();

            var repo = new Core.Data.Persistor.Invoice();

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(userId, "test-username", "testuser@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(clientId, userId, "client-company"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Invoice.GetInsertScript(invoiceId, clientId, DateTime.Now, Guid.Empty, null, "INV_0098"));

            var sqlFormatter = "IF NOT EXISTS (" +
                               "SELECT COUNT(*) FROM Invoices " +
                               "WHERE Id = '{0}' AND User_Id IS NULL " +
                               "HAVING COUNT(*) = 1" +
                               ") RAISERROR ('Error updating invoice.',16,1);";

            repo.DbContext.Database.ExecuteSqlCommand(string.Format(sqlFormatter, invoiceId));
            
            repo.UpdateUserId(userId, invoiceId);

            sqlFormatter = "IF NOT EXISTS (" +
                           "SELECT COUNT(*) FROM Invoices " +
                           "WHERE Id = '{0}' AND User_Id = '{1}' " +
                           "HAVING COUNT(*) = 1" +
                           ") RAISERROR ('Error updating user_id on invoice.',16,1);";

            repo.DbContext.Database.ExecuteSqlCommand(string.Format(sqlFormatter, invoiceId, userId));
        }
    }
}