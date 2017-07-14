using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StakHappy.Core.UnitTest.Validation.UserValidator
{
    [Collection("Validation")]
    public class ValidateFixture
    {
        [Fact]
        public void FailedOn_AllRequiredProperties()
        {
            var user = new Core.Data.Model.User();
            var result = new Core.Validation.UserValidator().Validate(user);

            Assert.Equal(result.IsValid, false);
            Assert.Equal(result.Errors.Count, 3);
            Assert.Equal(result.Errors[0].PropertyName, "Id");
            Assert.Equal(result.Errors[1].PropertyName, "UserName");
            Assert.Equal(result.Errors[2].PropertyName, "Email");
        }

        [Fact]
        public void FailedOn_IdIsRequired()
        {
            var user = new Core.Data.Model.User { UserName = "username", Email = "email" };
            var result = new Core.Validation.UserValidator().Validate(user);

            Assert.Equal(result.IsValid, false);
            Assert.Equal(result.Errors.Count, 1);
            Assert.Equal(result.Errors[0].PropertyName, "Id");
        }

        [Fact]
        public void FailedOn_UserNameIsRequired()
        {
            var user = new Core.Data.Model.User { Id = Guid.NewGuid(), Email = "email" };
            var result = new Core.Validation.UserValidator().Validate(user);

            Assert.Equal(result.IsValid, false);
            Assert.Equal(result.Errors.Count, 1);
            Assert.Equal(result.Errors[0].PropertyName, "UserName");
        }

        [Fact]
        public void FailedOn_EmailIsRequired()
        {
            var user = new Core.Data.Model.User { Id = Guid.NewGuid(), UserName="username" };
            var result = new Core.Validation.UserValidator().Validate(user);

            Assert.Equal(result.IsValid, false);
            Assert.Equal(result.Errors.Count, 1);
            Assert.Equal(result.Errors[0].PropertyName, "Email");
        }

        [Fact]
        public void Successful()
        {
            var user = new Core.Data.Model.User { Id = Guid.NewGuid(), UserName = "username", Email = "email" };
            var result = new Core.Validation.UserValidator().Validate(user);

            Assert.Equal(result.IsValid, true);
            Assert.Equal(result.Errors.Count, 0);
        }
    }
}
