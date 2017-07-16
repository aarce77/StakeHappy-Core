using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserServiceLogic
{
    [Collection("Logic")]
    public class SaveFixture : TestBase
    {
        [Fact]
        public void UserIdEmpty()
        {
            var service = new Core.Data.Model.UserService();

            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.UserServiceLogic().Save(service));

            Assert.Equal("user id most be specified to save a user service", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            var service = new Core.Data.Model.UserService()
            {
                User_Id = Guid.NewGuid()
            };

            // mocks
            var userServicePersistor = Mocks.StrictMock<Core.Data.Persistor.UserService>();
            var bll = Mocks.StrictMock<Core.Logic.UserServiceLogic>(userServicePersistor);

            // record
            bll.Expect(b => b.Save(service)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            userServicePersistor.Expect(u => u.Save(service)).Return(service);
            userServicePersistor.Expect(u => u.Commit()).Return(1);

            Mocks.ReplayAll();
            var result = bll.Save(service);

            Assert.Equal(service.Id, result.Id);

            Mocks.VerifyAll();
        }
    }
}