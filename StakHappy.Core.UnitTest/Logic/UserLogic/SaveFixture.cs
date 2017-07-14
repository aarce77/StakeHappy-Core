using System;
using Xunit;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;

namespace StakHappy.Core.UnitTest.Logic.UserLogic
{
    [Collection("Logic")]
    public class SaveFixture : TestBase
    {
        [Fact]
        public void FailureOn_ModelValidation()
        {
            // data
            var user = new Core.Data.Model.User
            {
                Id = Guid.NewGuid(),
                UserName = "aarce",
                FirstName = "Angel",
                LastName = "Arce",
                DisplayName = "A.Arce",
                Active = true,
                CreatedDate = DateTime.Now
            };
            var validatorResult = new Core.Validation.ValidatorResult(false)
            {
                Errors = { new Core.Validation.ValidatorFailure {
                    Message = "Email is requried", PropertyName = "Email" },
                    new Core.Validation.ValidatorFailure {
                        Message = "UserName is requried", PropertyName = "UserName" }}
            };

            // mocks
            var validator = Mocks.StrictMock<Core.Validation.UserValidator>();
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor, validator);

            // record
            bll.Expect(b => b.Save(user)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            validator.Expect(v => v.Validate(user)).Return(validatorResult);

            Mocks.ReplayAll();

            var ex = Assert.Throws<ArgumentException>(() => bll.Save(user));

            Assert.Equal("One or more properties a required\r\nParameter name: Email, UserName", ex.Message);
            Assert.Equal("Email, UserName", ex.ParamName);

            Mocks.VerifyAll();
        }

        [Fact]
        public void Successful()
        {
            // data
            var user = new Core.Data.Model.User
            {
                Id = Guid.NewGuid(),
                UserName = "aarce",
                FirstName = "Angel",
                LastName = "Arce",
                DisplayName = "A.Arce",
                Active = true,
                CreatedDate = DateTime.Now
            };

            // mocks
            var validator = Mocks.StrictMock<Core.Validation.UserValidator>();
            var persistor = Mocks.StrictMock<Core.Data.Persistor.User>();
            var bll = Mocks.StrictMock<Core.Logic.UserLogic>(persistor, validator);

            // record
            bll.Expect(b => b.Save(user)).CallOriginalMethod(OriginalCallOptions.NoExpectation);
            validator.Expect(v => v.Validate(user)).Return(new Core.Validation.ValidatorResult(true));
            persistor.Expect(u => u.Save(user)).Return(user);
            persistor.Expect(u => u.Commit()).Return(1);

            Mocks.ReplayAll();
            var result = bll.Save(user);

            Assert.Equal(user.Id, result.Id);

            Mocks.VerifyAll();
        }
    }
}
