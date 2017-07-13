using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.Invoice
{
    [Collection("Data")]
    public class SearchFixture : PersistorBase
    {
        #region Constructor
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _clientId = Guid.NewGuid(); 

        public SearchFixture()
        {
            SetupTestData();
        }
        #endregion

        #region Filtered
        [Fact]
        public void ByUser()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria {UserId = _userId, HasBalanceOnly = false};

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(8, coll.Count);
        }

        [Fact]
        public void ByUserPaged()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                PageSize = 4,
                Page = 2
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(4, coll.Count);
            Assert.NotNull(coll.Find(c => c.Number == "INV_0005"));
            Assert.NotNull(coll.Find(c => c.Number == "INV_0006"));
            Assert.NotNull(coll.Find(c => c.Number == "INV_0007"));
            Assert.NotNull(coll.Find(c => c.Number == "INV_0008"));
        }

        [Fact]
        public void ByUserAndHasBalanceOnly()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria { UserId = _userId };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(4, coll.Count);
        }

        [Fact]
        public void ByUserAndClient()
        {
            var clientId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();

            InsertAdditionalClientAndInvoice(clientId, invoiceId);

            var repo = new Core.Data.Persistor.Invoice();

            var criteria = new Core.Data.Search.InvoiceCriteria { UserId = _userId, HasBalanceOnly = false, ClientId = clientId };
            var coll = repo.Search(criteria).ToList();

            Assert.Equal(1, coll.Count);
            Assert.NotNull(coll.Find(i => i.Id == invoiceId));
        }

        [Fact]
        public void ByUserClientAndNumber()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                Number = "INV_0001"
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(1, coll.Count);
            Assert.NotNull(coll.Find(i => i.Number == "INV_0001"));
        }

        [Fact]
        public void ByUserAndInvoiceDateRange()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                InvoiceDateRange = { From = DateTime.Parse("10/25/2015"), To = DateTime.Parse("11/2/2015") }
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(3, coll.Count);
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{05CCDE2B-2637-4DFC-83DB-D92B3EB145D6}")));
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{6BBE344A-EBF9-404F-9130-C2B6AB23C5E1}")));
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{DE1317C4-0A7E-4E07-98C7-350E6AFD8CF3}")));
        }

        [Fact]
        public void ByUserAndVoided()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                Voided = true
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(2, coll.Count);
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{2E046739-7C02-4E1D-8FD9-2C3C49F960C2}")));
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{97DB6BD9-E314-4463-BBF8-BCCB9C56F903}")));
        }

        [Fact]
        public void ByUserAndVoidedDateRange()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                Voided = true,
                VoidedDateRange = { From = DateTime.Parse("10/16/2015"), To = DateTime.Parse("10/17/2015")}
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(1, coll.Count);
            Assert.NotNull(coll.Find(i => i.Id == Guid.Parse("{2E046739-7C02-4E1D-8FD9-2C3C49F960C2}")));
        }
        #endregion

        #region Sorted
        [Fact]
        public void SortByNumberDesc()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                SortBy = Core.Data.Search.InvoiceCriteria.SortFields.Number,
                SortOrder = Core.Data.SortDirection.DESC
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(8, coll.Count);
            Assert.True(coll[0].Number == "INV_0008");
            Assert.True(coll[1].Number == "INV_0007");
            Assert.True(coll[2].Number == "INV_0006");
            Assert.True(coll[3].Number == "INV_0005");
            Assert.True(coll[4].Number == "INV_0004");
            Assert.True(coll[5].Number == "INV_0003");
            Assert.True(coll[6].Number == "INV_0002");
            Assert.True(coll[7].Number == "INV_0001");
        }

        [Fact]
        public void SortByClientAsc()
        {
            var clientId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();

            InsertAdditionalClientAndInvoice(clientId, invoiceId);

            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                SortBy = Core.Data.Search.InvoiceCriteria.SortFields.Client,
                SortOrder = Core.Data.SortDirection.ASC
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(9, coll.Count);
            Assert.True(coll[0].Client.CompanyName == "Company_client-company");
            Assert.True(coll[1].Client.CompanyName == "Company_client-company");
            Assert.True(coll[2].Client.CompanyName == "Company_client-company");
            Assert.True(coll[3].Client.CompanyName == "Company_client-company");
            Assert.True(coll[4].Client.CompanyName == "Company_client-company");
            Assert.True(coll[5].Client.CompanyName == "Company_client-company");
            Assert.True(coll[6].Client.CompanyName == "Company_client-company");
            Assert.True(coll[7].Client.CompanyName == "Company_client-company");
            Assert.True(coll[8].Client.CompanyName == "Company_client-company3");
        }

        [Fact]
        public void SortByClientDesc()
        {
            var clientId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();

            InsertAdditionalClientAndInvoice(clientId, invoiceId);

            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                SortBy = Core.Data.Search.InvoiceCriteria.SortFields.Client,
                SortOrder = Core.Data.SortDirection.DESC
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(9, coll.Count);
            Assert.True(coll[0].Client.CompanyName == "Company_client-company3");
            Assert.True(coll[1].Client.CompanyName == "Company_client-company");
            Assert.True(coll[2].Client.CompanyName == "Company_client-company");
            Assert.True(coll[3].Client.CompanyName == "Company_client-company");
            Assert.True(coll[4].Client.CompanyName == "Company_client-company");
            Assert.True(coll[5].Client.CompanyName == "Company_client-company");
            Assert.True(coll[6].Client.CompanyName == "Company_client-company");
            Assert.True(coll[7].Client.CompanyName == "Company_client-company");
            Assert.True(coll[8].Client.CompanyName == "Company_client-company");
        }

        [Fact]
        public void SortByDataAsc()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                SortBy = Core.Data.Search.InvoiceCriteria.SortFields.Date,
                SortOrder = Core.Data.SortDirection.ASC
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(8, coll.Count);
            Assert.True(coll[0].Date == DateTime.Parse("10/15/2015"));
            Assert.True(coll[1].Date == DateTime.Parse("10/20/2015"));
            Assert.True(coll[2].Date == DateTime.Parse("10/25/2015"));
            Assert.True(coll[3].Date == DateTime.Parse("10/30/2015"));
            Assert.True(coll[4].Date == DateTime.Parse("11/1/2015"));
            Assert.True(coll[5].Date == DateTime.Parse("11/5/2015"));
            Assert.True(coll[6].Date == DateTime.Parse("11/10/2015"));
            Assert.True(coll[7].Date == DateTime.Parse("11/15/2015"));
        }

        [Fact]
        public void SortByDataDesc()
        {
            var repo = new Core.Data.Persistor.Invoice();
            var criteria = new Core.Data.Search.InvoiceCriteria
            {
                UserId = _userId,
                HasBalanceOnly = false,
                SortBy = Core.Data.Search.InvoiceCriteria.SortFields.Date,
                SortOrder = Core.Data.SortDirection.DESC
            };

            var coll = repo.Search(criteria).ToList();

            Assert.Equal(8, coll.Count);
            Assert.True(coll[0].Date == DateTime.Parse("11/15/2015"));
            Assert.True(coll[1].Date == DateTime.Parse("11/10/2015"));
            Assert.True(coll[2].Date == DateTime.Parse("11/5/2015"));
            Assert.True(coll[3].Date == DateTime.Parse("11/1/2015"));
            Assert.True(coll[4].Date == DateTime.Parse("10/30/2015"));
            Assert.True(coll[5].Date == DateTime.Parse("10/25/2015"));
            Assert.True(coll[6].Date == DateTime.Parse("10/20/2015"));
            Assert.True(coll[7].Date == DateTime.Parse("10/15/2015"));
        }
        #endregion

        #region Test Data Setup

        private void SetupTestData()
        {
            var repo = new Core.Data.Persistor.Invoice();

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(_userId, "test-username", "testuser@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(_clientId, _userId, "client-company"));

            var invoices = GetInvoiceData();

            var count = 1;
            foreach (var data in invoices)
            {
                InsertInvoices(repo.DbContext.Database, data.Key, data.Value.InvoiceDate, data.Value.Amount,
                    data.Value.Quantity, null, count % 2 == 0, data.Value.Number);

                count++;
            }

            // insert voided inovices
            InsertInvoices(repo.DbContext.Database, Guid.Parse("{2E046739-7C02-4E1D-8FD9-2C3C49F960C2}"),
                DateTime.Parse("10/16/2015"), 3250, 1, DateTime.Parse("10/17/2015"));

            InsertInvoices(repo.DbContext.Database, Guid.Parse("{97DB6BD9-E314-4463-BBF8-BCCB9C56F903}"),
                DateTime.Parse("10/21/2015"), 3250, 2, DateTime.Parse("10/22/2015"));


            // extra user to verify we don't retrive data for a different user
            var userId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.User.GetInsertScript(userId, "test-username2", "test-user2@tester.com"));

            repo.DbContext.Database.ExecuteSqlCommand(
                Utils.Client.GetInsertScript(clientId, userId, "client-company2"));

            // insert invoice
            repo.DbContext.Database.ExecuteSqlCommand(Utils.Invoice.GetInsertScript(invoiceId, clientId, DateTime.Parse("10/17/2015"), userId));

            // insert invoice item
            repo.DbContext.Database.ExecuteSqlCommand(Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, 2, 1595));

            // insert payment
            repo.DbContext.Database.ExecuteSqlCommand(Utils.Payment.GetInsertScript(Guid.NewGuid(), invoiceId, (2 * 1595)));

        }

        private void InsertInvoices(System.Data.Entity.Database db, Guid invoiceId, DateTime invoiceDate, long amount, 
            int quantity, DateTime? voidedDate = null, bool hasBalance = false, string number = null)
        {
            // insert invoice
            db.ExecuteSqlCommand(Utils.Invoice.GetInsertScript(invoiceId, _clientId, invoiceDate, _userId, voidedDate, number));

            // insert invoice item
            db.ExecuteSqlCommand(Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, quantity, amount));
            
            long payment = (quantity * amount);
            if (hasBalance)
            {
                if(quantity == 1)
                    payment = (amount / 2);
                else
                    payment = amount;
            }
                
            // insert payment
            db.ExecuteSqlCommand(Utils.Payment.GetInsertScript(Guid.NewGuid(), invoiceId, payment));
        }

        private void InsertAdditionalClientAndInvoice(Guid clientId, Guid invoiceId)
        {
            var repo = new Core.Data.Persistor.Invoice();

            // insert client
            repo.DbContext.Database.ExecuteSqlCommand(Utils.Client.GetInsertScript(clientId, _userId, "client-company3"));
            // insert invoice
            repo.DbContext.Database.ExecuteSqlCommand(Utils.Invoice.GetInsertScript(invoiceId, clientId, DateTime.Parse("10/17/2015"), _userId));
            // insert invoice item
            repo.DbContext.Database.ExecuteSqlCommand(Utils.InvoiceItem.GetInsertScript(Guid.NewGuid(), invoiceId, 3, 1595));
            // insert payment
            repo.DbContext.Database.ExecuteSqlCommand(Utils.Payment.GetInsertScript(Guid.NewGuid(), invoiceId, (2 * 1595)));
        }

        private Dictionary<Guid, InvoiceData> GetInvoiceData()
        {
            var invoices = new Dictionary<Guid, InvoiceData>
            {
                {
                    Guid.Parse("{7F67C515-41AB-48B4-9459-A55DB9AF51F6}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{7F67C515-41AB-48B4-9459-A55DB9AF51F6}"),
                        Quantity = 2,
                        Amount = 1999,
                        InvoiceDate = DateTime.Parse("10/15/2015"),
                        Number = "INV_0002"
                    }
                },
                {
                    Guid.Parse("{265BBA88-0AE2-4DCB-9A64-CC054477BD04}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{265BBA88-0AE2-4DCB-9A64-CC054477BD04}"),
                        Quantity = 1,
                        Amount = 2500,
                        InvoiceDate = DateTime.Parse("10/20/2015"),
                        Number = "INV_0001"
                    }
                },
                {
                    Guid.Parse("{05CCDE2B-2637-4DFC-83DB-D92B3EB145D6}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{05CCDE2B-2637-4DFC-83DB-D92B3EB145D6}"),
                        Quantity = 3,
                        Amount = 1523,
                        InvoiceDate = DateTime.Parse("10/25/2015"),
                        Number = "INV_0003"
                    }
                },
                {
                    Guid.Parse("{6BBE344A-EBF9-404F-9130-C2B6AB23C5E1}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{6BBE344A-EBF9-404F-9130-C2B6AB23C5E1}"),
                        Quantity = 4,
                        Amount = 2500,
                        InvoiceDate = DateTime.Parse("10/30/2015"),
                        Number = "INV_0004"
                    }
                },
                {
                    Guid.Parse("{DE1317C4-0A7E-4E07-98C7-350E6AFD8CF3}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{DE1317C4-0A7E-4E07-98C7-350E6AFD8CF3}"),
                        Quantity = 6,
                        Amount = 1295,
                        InvoiceDate = DateTime.Parse("11/01/2015"),
                        Number = "INV_0005"
                    }
                },
                {
                    Guid.Parse("{74B23B10-4534-4E1F-8327-934F171106A7}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{74B23B10-4534-4E1F-8327-934F171106A7}"),
                        Quantity = 3,
                        Amount = 6050,
                        InvoiceDate = DateTime.Parse("11/05/2015")
                        ,
                        Number = "INV_0006"
                    }
                },
                {
                    Guid.Parse("{3BB6AD1F-71EA-4752-9A5D-65BD43912DB3}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{3BB6AD1F-71EA-4752-9A5D-65BD43912DB3}"),
                        Quantity = 1,
                        Amount = 2495,
                        InvoiceDate = DateTime.Parse("11/10/2015"),
                        Number = "INV_0007"
                    }
                },
                {
                    Guid.Parse("{5623ADAB-F0FD-4F6C-AFB0-C6025C22D1F8}"),
                    new InvoiceData
                    {
                        Id = Guid.Parse("{5623ADAB-F0FD-4F6C-AFB0-C6025C22D1F8}"),
                        Quantity = 2,
                        Amount = 7500,
                        InvoiceDate = DateTime.Parse("11/15/2015"),
                        Number = "INV_0008"
                    }
                }
            };

            return invoices;
        }
        
        public class InvoiceData
        {
            public Guid Id { get; set; }
            public int Quantity { get; set; }
            public long Amount { get; set; }
            public DateTime InvoiceDate { get; set; }
            public DateTime VoidedDate { get; set; }
            public string Number { get; set; }
        }

        #endregion
    }
}