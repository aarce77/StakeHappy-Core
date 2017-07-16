using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserServiceLogic
{
    [Collection("Logic")]
    public class GetListFixture : TestBase
    {
        [Fact]
        public void Successful()
        {
            // data
            var id = Guid.NewGuid();
            System.Linq.Expressions.Expression<Func<Core.Data.Model.UserService, bool>> expr = u => u.Id ==  id;
            var services = new List<Core.Data.Model.UserService>();

            // mocks
            var userServicePersistor = Mocks.StrictMock<Core.Data.Persistor.UserService>();
            var bll = Mocks.StrictMock<Core.Logic.UserServiceLogic>(userServicePersistor);

            bll.Expect(b => b.GetList(expr)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            userServicePersistor.Expect(d => d.Get(expr)).Return(services.AsQueryable());

            // record
            Mocks.ReplayAll();

            var results = bll.GetList(expr);

            Assert.Equal(services, results);

        }
    }
}