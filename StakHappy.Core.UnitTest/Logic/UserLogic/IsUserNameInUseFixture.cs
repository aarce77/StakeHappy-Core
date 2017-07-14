using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserLogic
{
    [Collection("Logic")]
    public class IsUserNameInUseFixture : TestBase
    {
        [Fact]
        public void ThrowsArgumentNullException_UsernameEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => 
                new Core.Logic.UserLogic().IsUserNameInUse(String.Empty));

            Assert.Equal("username", ex.ParamName);
        }

        [Fact]
        public void SuccessfulWithoutUserId()
        {
            // mocks
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor, null);

            bll.Expect(b => b.IsUserNameInUse("username"))
                .CallOriginalMethod(OriginalCallOptions.NoExpectation);
            persistor.Expect(d => d.IsUserNameInUse("username", Guid.Empty)).Return(false);

            // record
            Mocks.ReplayAll();

            var result = bll.IsUserNameInUse("username");

            Assert.False(result);
        }

        [Fact]
        public void SuccessfulWithUserId()
        {
            // data
            var userId = Guid.NewGuid();
            var user = new Core.Data.Model.User { Id = userId };

            // mocks
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor, null);

            bll.Expect(b => b.IsUserNameInUse("username", user.Id))
                .CallOriginalMethod(OriginalCallOptions.NoExpectation);
            persistor.Expect(d => d.IsUserNameInUse("username", user.Id)).Return(true);

            // record
            Mocks.ReplayAll();

            var result = bll.IsUserNameInUse("username", user.Id);

            Assert.True(result);
        }
    }
}
