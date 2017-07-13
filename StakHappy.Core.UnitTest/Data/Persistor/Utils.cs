using System;

namespace StakHappy.Core.UnitTest.Data.Persistor
{
    public static class Utils
    {
        public static class User
        {
            public static string GetInsertScript()
            {
                return "INSERT INTO [Users] ([Id],[UserName],[FirstName],[LastName],[DisplayName],[Active],[Email],[CreatedDate]) " +
                    "VALUES ('{39E50EDC-AA66-42AB-9D92-858E628E2FAC}','tester','Test','Tester','Test',1,'tester@test.com',GETDATE())";
            }

            public static string GetInsertScript(Guid id, string username, string email)
            {
                return string.Format("INSERT INTO Users " +
                    "(Id, Username, FirstName, LastName, DisplayName, Active, Email, CreatedDate) " +
                    "VALUES('{0}','{1}','Test','Tester','Tester',1, '{2}', GETDATE())", id, username, email);
            }
        }

        public static class Client
        {
            public static string GetInsertScript(Guid id, Guid userId, string companyName = null)
            {
                return string.Format("INSERT INTO Clients " +
                    "(Id, User_Id, CompanyName, Active, CreatedDate) " +
                    "VALUES('{0}','{1}','Company_{2}', 1, GETDATE())", id, userId, 
                    string.IsNullOrEmpty(companyName) ? id.ToString() : companyName);
            }
        }

        public static class ClientContact
        {
            public static string GetInsertScript(Guid id, Guid clientId)
            {
                return string.Format("INSERT INTO ClientContacts " +
                    "(Id, Client_Id, IsPrimary, CreatedDate) " +
                    "VALUES('{0}','{1}', 1, GETDATE())", id, clientId);
            }
        }

        public static class Invoice
        {
            public static string GetInsertScript(Guid id, Guid clientId, DateTime invoiceDate, 
                Guid userId, DateTime? voidedDate = null, string number = null)
            {
                var voided = voidedDate == null ? 0 : 1;
                if (string.IsNullOrEmpty(number))
                    number = id.ToString();

                if (userId == Guid.Empty)
                {
                    return string.Format("INSERT INTO Invoices " +
                                         "(Id, Client_Id, Date, Active, CreatedDate, LastModified, VoidedDate, Voided, Number) " +
                                         "VALUES('{0}','{1}', '{2}', 1, GETDATE(), GETDATE(), '{3}', {4}, '{5}')",
                        id, clientId, invoiceDate, voidedDate, voided, number);
                }
                return string.Format("INSERT INTO Invoices " +
                                     "(Id, Client_Id, Date, Active, CreatedDate, LastModified, VoidedDate, Voided, User_Id, Number) " +
                                     "VALUES('{0}','{1}', '{2}', 1, GETDATE(), GETDATE(), '{3}', {4}, '{5}', '{6}')",
                    id, clientId, invoiceDate, voidedDate, voided, userId, number);

            }
        }

        public static class InvoiceItem
        {
            public static string GetInsertScript(Guid id, Guid invoiceId, int quantity, long unitCost)
            {
                return string.Format("INSERT INTO InvoiceItems " +
                    "(Id, Invoice_Id, Quantity, UnitCost, CreatedDate, LastModified) " +
                    "VALUES('{0}','{1}', {2}, {3}, GETDATE(), GETDATE())", id, invoiceId, quantity, unitCost);
            }
        }

        public static class Payment
        {
            public static string GetInsertScript(Guid id, Guid invoiceId, long amount)
            {
                return string.Format("INSERT INTO Payments " +
                    "(Id, Invoice_Id, Amount, Active, CreatedDate, LastModified) " +
                    "VALUES('{0}','{1}', {2}, 1, GETDATE(), GETDATE())", id, invoiceId, amount);
            }
        }

        public static class UserService
        {
            public static string GetInsertScript(Guid id, Guid userId, long price)
            {
                return string.Format("INSERT INTO UserServices " +
                    "(Id, User_Id, Price, CreatedDate, LastModified) " +
                    "VALUES('{0}','{1}', {2}, GETDATE(), GETDATE())", id, userId, price);
            }
        }

        public static class UserSetting
        {
            public static string GetInsertScript(Guid id, Guid userId, int dataType = 0)
            {
                return string.Format("INSERT INTO UserSettings " +
                    "(Id, User_Id, DataType, Active, CreatedData) " +
                    "VALUES('{0}','{1}', {2}, 1, GETDATE())", id, userId, dataType);
            }
        }
    }
}
