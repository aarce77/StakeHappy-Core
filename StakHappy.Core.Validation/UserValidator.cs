using System;
using FluentValidation;

namespace StakHappy.Core.Validation
{
    public class UserValidator : AbstractValidator<Data.Model.User>, IValidator
    {
        public UserValidator()
        {
            Inititalize();
        }

        public virtual ValidatorResult Validate(Data.Model.IEntity entity)
        {
            var vr = new ValidatorResult(true);

            var user = (entity as Data.Model.User);
            var results = base.Validate(user);

            vr.IsValid = results.IsValid;
            if (results.IsValid)
                return vr;

            foreach (var error in results.Errors)
            {
                vr.Errors.Add(new ValidatorFailure
                {
                    PropertyName = error.PropertyName,
                    Message = error.ErrorMessage
                });
            }

            return vr;
        }

        private void Inititalize()
        {
            RuleFor(u => u.Id).NotEqual(Guid.Empty).WithMessage("Id is required");
            RuleFor(u => u.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
