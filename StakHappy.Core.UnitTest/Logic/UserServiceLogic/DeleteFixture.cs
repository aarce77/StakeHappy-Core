using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserServiceLogic
{
    [Collection("Logic")]
    public class DeleteFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.UserServiceLogic().Delete(Guid.Empty));
            Assert.Equal("id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();

            // mocks
            var userServicePersistor = Mocks.StrictMock<Core.Data.Persistor.UserService>();
            var bll = Mocks.StrictMock<Core.Logic.UserServiceLogic>(userServicePersistor);

            bll.Expect(b => b.Delete(id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            userServicePersistor.Expect(d => d.Delete(id));
            userServicePersistor.Expect(d => d.Commit()).Return(1);

            // record
            Mocks.ReplayAll();

            bll.Delete(id);
        }
         
    }
}