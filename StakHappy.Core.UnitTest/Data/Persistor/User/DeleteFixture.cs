using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.User
{
    [Collection("Data")]
    public class DeleteFixture : PersistorBase
    {
        private readonly Guid _userId;
        public DeleteFixture()
        {
            _userId = Guid.NewGuid();

            var persistor = new Core.Data.Persistor.User();
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(_userId, "test-username", "testuser@tester.com"));
        }

        [Fact]
        public void UserOnly()
        {
            VerifyDelete(string.Empty);
        }

        [Fact]
        public void UserWithClients()
        {
            VerifyDelete(
                Utils.Client.GetInsertScript(Guid.NewGuid(), _userId));
        }

        [Fact]
        public void UserWithServices()
        {
            VerifyDelete(
                Utils.UserService.GetInsertScript(Guid.NewGuid(), _userId, 25000));

        }

        [Fact]
        public void UserWithSetting()
        {
            VerifyDelete(Utils.UserSetting.GetInsertScript(Guid.NewGuid(), _userId));
        }

        [Fact]
        public void Full()
        {
            var clientId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var persistor = new Core.Data.Persistor.User();

            // insert client
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(clientId, _userId));

            // insert client contact
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.ClientContact.GetInsertScript(Guid.NewGuid(), clientId));

            // insert invoice
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.Invoice.GetInsertScript(invoiceId, clientId, DateTime.Now, _userId));

            // insert invoice item
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, 2, 320000));

            // insert payment
            persistor.DbContext.Database.ExecuteSqlCommand(
                Utils.Payment.GetInsertScript(Guid.NewGuid(), invoiceId, 640000));

            VerifyDelete(string.Empty);
        }

        private void VerifyDelete(string sql)
        {
            var persistor = new Core.Data.Persistor.User();
            if(!string.IsNullOrEmpty(sql))
                persistor.DbContext.Database.ExecuteSqlCommand(sql);

            persistor.Delete(_userId);
            persistor.Commit();

            const string sqlFormatter = "IF EXISTS (" +
                                        "SELECT COUNT(*) FROM Users WHERE Id = '{0}' HAVING COUNT(*) > 0" +
                                        ") RAISERROR ('Error deleting user.',16,1);";

            var deleteCheckSql = string.Format(sqlFormatter, _userId);
            persistor.DbContext.Database.ExecuteSqlCommand(deleteCheckSql);
        }
    }
}