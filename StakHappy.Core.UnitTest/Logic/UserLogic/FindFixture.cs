using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace StakHappy.Core.UnitTest.Logic.UserLogic
{
    [Collection("Logic")]
    public class FindFixture : TestBase
    {
        [Fact]
        public void Successful()
        {
            // data
            var users = new List<Core.Data.Model.User>().AsQueryable();

            // mocks
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor,null);

            persistor.Expect(d => d.Get(u => u.CompanyName == "company")).IgnoreArguments().Return(users);
            bll.Expect(b => b.Find(u => u.CompanyName == "company"))
                .CallOriginalMethod(OriginalCallOptions.NoExpectation);

            // record
            Mocks.ReplayAll();

            var result = bll.Find(u => u.CompanyName == "company");

            Assert.IsType(typeof(EnumerableQuery<Core.Data.Model.User>), result);
        }
    }
}
