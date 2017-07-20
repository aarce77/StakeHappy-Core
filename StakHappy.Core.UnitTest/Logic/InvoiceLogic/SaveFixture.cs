using System;
using System.Collections.Generic;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.InvoiceLogic
{
    [Collection("Logic")]
    public class SaveFixture : TestBase
    {
        [Fact]
        public void UserIdEmpty()
        {
            var invoice = new Core.Data.Model.Invoice();

            var ex = Assert.Throws<ArgumentException>(() => 
                new Core.Logic.InvoiceLogic().Save(Guid.Empty, invoice));

            Assert.Equal("User id most be specified to save an invoice", ex.Message);
        }

        [Fact]
        public void ClientIdEmpty()
        {
            var invoice = new Core.Data.Model.Invoice();

            var ex = Assert.Throws<ArgumentException>(() => 
                new Core.Logic.InvoiceLogic().Save(Guid.NewGuid(), invoice));

            Assert.Equal("Client id most be specified to save an invoice", ex.Message);
        }

        [Fact]
        public void SuccessfulInsertWithUserIdLookup()
        {
            // data
            var userId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var client = new Core.Data.Model.Client { Id = clientId, User_Id = userId };
            var invoice = new Core.Data.Model.Invoice
            {
                Id = Guid.Empty,
                Client_Id = clientId,
                Active = true,
                CreatedDate = DateTime.Now,
                Items = new List<Core.Data.Model.InvoiceItem>
                {
                    new Core.Data.Model.InvoiceItem
                    {
                        Id = Guid.NewGuid()
                    },
                    new Core.Data.Model.InvoiceItem
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var invoiceItemPersistor = Mocks.StrictMock<Core.Data.Persistor.InvoiceItem>();
            var clientPersistor = Mocks.StrictMock<Core.Data.Persistor.Client>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor, invoiceItemPersistor, clientPersistor);

            // record
            bll.Expect(b => b.Save(invoice)).CallOriginalMethod(OriginalCallOptions.NoExpectation);

            clientPersistor.Expect(c => c.Get(invoice.Client_Id)).Return(client);
            bll.Expect(b => b.Save(userId, invoice)).Return(invoice);

            Mocks.ReplayAll();
            var result = bll.Save(invoice);

            Assert.Equal(invoice.Id, result.Id);
        }

        [Fact]
        public void SuccessfulInsert()
        {
            // data
            var userId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var invoice = new Core.Data.Model.Invoice
            {
                Id = Guid.Empty,
                Client_Id = Guid.NewGuid(),
                Active = true,
                CreatedDate = DateTime.Now,
                Items = new List<Core.Data.Model.InvoiceItem>
                {
                    new Core.Data.Model.InvoiceItem
                    {
                        Id = Guid.NewGuid()
                    },
                    new Core.Data.Model.InvoiceItem
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var invoiceItemPersistor = Mocks.StrictMock<Core.Data.Persistor.InvoiceItem>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor, invoiceItemPersistor, null);
            
            // record
            bll.Expect(b => b.Save(userId, invoice)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoicePersistor.Expect(u => u.Save(invoice)).Return(invoice).WhenCalled(c => invoice.Id = invoiceId);
            invoicePersistor.Expect(u => u.Commit()).Return(1);
            invoiceItemPersistor.Expect(
                c => c.Save(Arg<Core.Data.Model.InvoiceItem>.Matches(m => m.Invoice_Id == invoice.Id && m.Invoice_Id != Guid.Empty)))
                .Repeat.Twice();
            invoiceItemPersistor.Expect(u => u.Commit()).Return(1);
            invoicePersistor.Expect(i => i.UpdateUserId(userId, invoiceId));

            Mocks.ReplayAll();
            var result = bll.Save(userId, invoice);

            Assert.Equal(invoice.Id, result.Id);
        }

        [Fact]
        public void SuccessfulUpdate()
        {
            // data
            var userId = Guid.NewGuid();
            var invoice = new Core.Data.Model.Invoice
            {
                Id = Guid.NewGuid(),
                Client_Id = Guid.NewGuid(),
                Active = true,
                CreatedDate = DateTime.Now,
                Items = new List<Core.Data.Model.InvoiceItem>
                {
                    new Core.Data.Model.InvoiceItem
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            // mocks
            var invoicePersistor = Mocks.StrictMock<Core.Data.Persistor.Invoice>();
            var bll = Mocks.StrictMock<Core.Logic.InvoiceLogic>(invoicePersistor,null, null);

            // record
            bll.Expect(b => b.Save(userId, invoice)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            invoicePersistor.Expect(u => u.Save(invoice)).Return(invoice);
            invoicePersistor.Expect(i => i.Commit()).Return(1);
            invoicePersistor.Expect(i => i.UpdateUserId(userId, invoice.Id));
            Mocks.ReplayAll();
            var result = bll.Save(userId, invoice);

            Assert.Equal(invoice.Id, result.Id);
        }
    }
}