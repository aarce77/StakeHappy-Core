using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Client
{
    [Collection("Data")]
    public class DeleteFixture : PersistorBase
    {
        readonly Guid _clientId = Guid.NewGuid();
        readonly Guid _userId = Guid.NewGuid();

        public DeleteFixture()
        {
            _clientId = Guid.NewGuid();

            var repo = new Core.Data.Persistor.Client();

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(_userId, "test-username", "testuser@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(_clientId, _userId, "client-company"));
        }

        [Fact]
        public void Simple()
        {
            VerifyDelete();
        }

        [Fact]
        public void WithContacts()
        {
            var repo = new Core.Data.Persistor.Client();

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.ClientContact.GetInsertScript(Guid.NewGuid(), _clientId));

            VerifyDelete();
        }

        [Fact]
        public void WithInvoice()
        {
            var invoiceId = Guid.NewGuid();
            var repo = new Core.Data.Persistor.Client();
            // insert invoice
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Invoice.GetInsertScript(invoiceId, _clientId, DateTime.Now, _userId));

            // insert invoice item
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, 2, 320000));

            VerifyDelete();
        }

        [Fact]
        public void WithInvoiceAndPayment()
        {
            var invoiceId = Guid.NewGuid();
            var repo = new Core.Data.Persistor.Client();
            // insert invoice
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Invoice.GetInsertScript(invoiceId, _clientId, DateTime.Now, _userId));

            // insert invoice item
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, 2, 320000));

            // insert payment
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Payment.GetInsertScript(Guid.NewGuid(), invoiceId, 640000));

            VerifyDelete();
        }

        [Fact]
        private void VerifyDelete()
        {
            var repo = new Core.Data.Persistor.Client();
            var sql = string.Format("IF NOT EXISTS (" +
                                    "SELECT COUNT(*) FROM Clients WHERE Id = '{0}' HAVING COUNT(*) > 0" +
                                    ") RAISERROR ('Client not found.',16,1);", _clientId);

            repo.DbContext.Database.ExecuteSqlCommand(sql);

            repo.Delete(_clientId);
            repo.Commit();

            sql = string.Format("IF EXISTS (" +
                                    "SELECT COUNT(*) FROM Clients WHERE Id = '{0}' HAVING COUNT(*) > 0" +
                                    ") RAISERROR ('Error deleting client.',16,1);", _clientId);

            repo.DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}