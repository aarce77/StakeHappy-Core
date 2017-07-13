using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.User
{
    [Collection("Data")]
    public class SaveFixture : PersistorBase
    {
        [Fact]
        public void Successful()
        {
            var userId = Guid.Empty;
            var user = new Core.Data.Model.User
            {
                Id          = userId,
                UserName    = "tester",
                FirstName   = "Test",
                LastName    = "Tester",
                DisplayName = "Tester",
                Email       = "test@tester.com",
                Active      = true,
                CreatedDate = DateTime.Parse("06/16/2013")
            };

            var persistor = new Core.Data.Persistor.User();
            persistor.Save(user);
            persistor.Commit();

            Assert.NotEqual(Guid.Empty, user.Id);

            const string sqlFormatter = "IF NOT EXISTS (" +
                                        "SELECT COUNT(*) FROM Users WHERE Id = '{0}' HAVING COUNT(*) = 1" +
                                        ") RAISERROR ('Error creating user.',16,1);";

            var sql = string.Format(sqlFormatter, user.Id);
            persistor.DbContext.Database.ExecuteSqlCommand(sql);
        }

        [Fact]
        public void DuplicateUserName()
        {
            var persistor = new Core.Data.Persistor.User();

            var sql = Utils.User.GetInsertScript(Guid.Parse("{5CEAD906-4825-4357-A60D-F0363B247CA6}"), 
                "DuplicateUserName", "test@tester.com");

            persistor.DbContext.Database.ExecuteSqlCommand(sql);

            sql = "IF NOT EXISTS (" +
                "SELECT COUNT(*) FROM Users WHERE Id = '{5CEAD906-4825-4357-A60D-F0363B247CA6}' " + 
                "HAVING COUNT(*) = 1) RAISERROR ('Error creating user.',16,1); ";

            persistor.DbContext.Database.ExecuteSqlCommand(sql);

            var userId = Guid.Empty;
            var user = new Core.Data.Model.User
            {
                Id          = userId,
                UserName = "DuplicateUserName",
                FirstName   = "Test",
                LastName    = "Tester",
                DisplayName = "Tester",
                Active      = true,
                Email       = "tester@test.com",
                CreatedDate = DateTime.Now
            };

            persistor.Save(user);
            Exception ex = Assert.Throws<System.Data.Entity.Infrastructure.DbUpdateException>(() => persistor.Commit());

            Assert.NotNull(ex.InnerException.InnerException);
            Assert.True(ex.InnerException.InnerException.Message.Contains("IX_User_UserName"));
            Assert.True(ex.InnerException.InnerException.Message.Contains("DuplicateUserName"));
        }

        [Fact]
        public void DuplicateEmail()
        {
            var persistor = new Core.Data.Persistor.User();

            var sql = Utils.User.GetInsertScript(Guid.Parse("{5CEAD906-4825-4357-A60D-F0363B247CA6}"),
                "tester12", "tester@test.com");

            persistor.DbContext.Database.ExecuteSqlCommand(sql);

            sql = "IF NOT EXISTS (" +
                "SELECT COUNT(*) FROM Users WHERE Id = '{5CEAD906-4825-4357-A60D-F0363B247CA6}' " +
                "HAVING COUNT(*) = 1) RAISERROR ('Error creating user.',16,1); ";

            persistor.DbContext.Database.ExecuteSqlCommand(sql);

            var userId = Guid.Empty;
            var user = new Core.Data.Model.User
            {
                Id          = userId,
                UserName    = "tester",
                FirstName   = "Test",
                LastName    = "Tester",
                DisplayName = "Tester",
                Active      = true,
                Email       = "tester@test.com",
                CreatedDate = DateTime.Now
            };

            persistor.Save(user);
            Exception ex = Assert.Throws<System.Data.Entity.Infrastructure.DbUpdateException>(() => persistor.Commit());

            Assert.NotNull(ex.InnerException.InnerException);
            Assert.True(ex.InnerException.InnerException.Message.Contains("IX_User_Email"));
            Assert.True(ex.InnerException.InnerException.Message.Contains("tester@test.com"));
        }
    }
}
