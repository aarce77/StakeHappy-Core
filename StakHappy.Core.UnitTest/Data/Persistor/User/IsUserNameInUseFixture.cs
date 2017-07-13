using System;
using Xunit;

namespace StakHappy.Core.UnitTest.Data.Persistor.User
{
    [Collection("Data")]
    public class IsUserNameInUseFixture : PersistorBase
    {
        public IsUserNameInUseFixture()
            : base()
        {
            var persistor = new Core.Data.Persistor.User();
            persistor.DbContext.Database.ExecuteSqlCommand(Utils.User.GetInsertScript());
        }

        [Fact]
        public void NotInUse()
        {
            var isUse = new Core.Data.Persistor.User().IsUserNameInUse("new-tester");

            Assert.False(isUse);
        }

        [Fact]
        public void InUser()
        {
            var isUse = new Core.Data.Persistor.User().IsUserNameInUse("tester");

            Assert.True(isUse);
        }

        [Fact]
        public void NotInUseWithUserIdProvided()
        {
            var isUse = new Core.Data.Persistor.User().IsUserNameInUse("tester", Guid.Parse("{39E50EDC-AA66-42AB-9D92-858E628E2FAC}"));

            Assert.False(isUse);
        }

        [Fact]
        public void InUseWithUserIdProvided()
        {
            var isUse = new Core.Data.Persistor.User().IsUserNameInUse("tester", Guid.NewGuid());

            Assert.True(isUse);
        }
    }
}
