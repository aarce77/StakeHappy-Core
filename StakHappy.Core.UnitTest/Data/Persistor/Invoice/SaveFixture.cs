using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Invoice
{
    [Collection("Data")]
    public class SaveFixture : PersistorBase
    {
        private readonly Guid _userId;
        private readonly Guid _clientId;
        public SaveFixture()
        {
            _userId = Guid.NewGuid();
            _clientId = Guid.NewGuid();

            var repo = new Core.Data.Persistor.User();
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(_userId, "test-username", "testuser@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(_clientId, _userId, "client-company"));
        }

        [Fact]
        public void SuccessfulInsert()
        {
            var invoiceId = Guid.Empty;
            var invoice = new Core.Data.Model.Invoice
            {
                Id = invoiceId,
                Active = true,
                Client_Id = _clientId,
                Date = DateTime.Now,
                Number = "INV_00098",
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            var repo = new Core.Data.Persistor.Invoice();
            repo.Save(invoice);
            repo.Commit();

            Assert.NotEqual(Guid.Empty, invoice.Id);

            const string sqlFormatter = "IF NOT EXISTS (" +
                                        "SELECT COUNT(*) FROM Invoices " +
                                        "WHERE Id = '{0}' " +
                                        "HAVING COUNT(*) = 1" +
                                        ") RAISERROR ('Error creating invoice.',16,1);";

            var sql = string.Format(sqlFormatter, invoice.Id, _userId);
            repo.DbContext.Database.ExecuteSqlCommand(sql);
        }

        [Fact]
        public void SuccessfulUpdate()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var invoiceId = Guid.NewGuid();
            var invoiceDate = DateTime.Now;
            var voidedDate = DateTime.Now;

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Invoice.GetInsertScript(invoiceId, _clientId, invoiceDate, _userId, null, "INV_0098"));

            var invoice = new Core.Data.Model.Invoice
            {
                Id = invoiceId,
                Active = true,
                Client_Id = _clientId,
                Date = invoiceDate,
                Number = "INV_00981",
                Voided = true,
                VoidedDate = voidedDate,
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            repo.Save(invoice);
            repo.Commit();

            Assert.NotEqual(Guid.Empty, invoice.Id);

            const string sqlFormatter = "IF NOT EXISTS (" +
                                        "SELECT COUNT(*) FROM Invoices " +
                                        "WHERE Id = '{0}' AND Number = '{1}' AND Voided = 1 AND DATEDIFF(DAY, VoidedDate, '{2}') = 0 " +
                                        "HAVING COUNT(*) = 1" +
                                        ") RAISERROR ('Error updating invoice.',16,1);";

            var sql = string.Format(sqlFormatter, invoice.Id, "INV_00981", voidedDate);
            repo.DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}