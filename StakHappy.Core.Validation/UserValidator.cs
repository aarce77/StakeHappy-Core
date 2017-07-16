using System;
using FluentValidation;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Validation
{
    public class UserValidator : ValidatorBase<User>, IValidator
    {
        public UserValidator()
        {
            Inititalize();
        }

        public virtual ValidatorResult Validate(IEntity entity)
        {
            return GetResult(base.Validate((entity as User)));
        }

        private void Inititalize()
        {
            RuleFor(u => u.Id).NotEqual(Guid.Empty).WithMessage("Id is required");
            RuleFor(u => u.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
