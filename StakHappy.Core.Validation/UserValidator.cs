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
            RuleFor(u => u.Id).NotEqual(Guid.Empty).When(u => u.CreatedDate.Date != DateTime.Now.Date).WithMessage("Id is required");  // this becomes a problem when obejct being saved is a new user
            RuleFor(u => u.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
