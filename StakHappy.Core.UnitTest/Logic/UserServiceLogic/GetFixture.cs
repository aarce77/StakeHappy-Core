using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserServiceLogic
{
    [Collection("Logic")]
    public class GetFixture : TestBase
    {
        [Fact]
        public void EmptyId()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Core.Logic.UserServiceLogic().Get(Guid.Empty));

            Assert.Equal("service id cannot be empty", ex.Message);
        }

        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            var service = new Core.Data.Model.UserService() { Id = id };

            // mocks
            var userServicePersistor = Mocks.StrictMock<Core.Data.Persistor.UserService>();
            var bll = Mocks.StrictMock<Core.Logic.UserServiceLogic>(userServicePersistor);

            bll.Expect(b => b.Get(service.Id)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            userServicePersistor.Expect(d => d.Get(service.Id)).Return(service);

            // record
            Mocks.ReplayAll();

            var result = bll.Get(id);

            Assert.Equal(id, result.Id);
        } 
    }
}