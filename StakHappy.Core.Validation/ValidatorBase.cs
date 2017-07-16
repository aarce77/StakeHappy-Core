using System;
using FluentValidation;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Validation
{
    public class ValidatorBase<T> : AbstractValidator<T>
    {
        internal virtual ValidatorResult GetResult(FluentValidation.Results.ValidationResult results)
        {
            var vr = new ValidatorResult(results.IsValid);

            if (vr.IsValid)
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
    }
}
