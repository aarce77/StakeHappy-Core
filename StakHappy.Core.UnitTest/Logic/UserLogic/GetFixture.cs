using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserLogic
{
    [Collection("Logic")]
    public class GetFixture : TestBase
    {
        [Fact]
        public void FailureOn_MissingProperty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.UserLogic().Get(Guid.Empty));

            Assert.Equal("user id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var userId = Guid.NewGuid();
            var user = new Core.Data.Model.User { Id = userId };

            // mocks
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor, null);

            bll.Expect(b => b.Get(user.Id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            persistor.Expect(d => d.Get(user.Id)).Return(user);

            // record
            Mocks.ReplayAll();

            var result = bll.Get(userId);

            Assert.Equal(userId, result.Id);
        }
    }
}
