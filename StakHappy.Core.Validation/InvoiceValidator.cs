using System;
using FluentValidation;
using StakHappy.Core.Data.Model;

namespace StakHappy.Core.Validation
{
    public class InvoiceValidator : ValidatorBase<Invoice>, IValidator
    {
        public InvoiceValidator()
        {
            Initialize();
        }

        public ValidatorResult Validate(IEntity entity)
        {
            return GetResult(base.Validate((entity as Invoice)));
        }

        private void Initialize()
        {
            RuleFor(i => i.Client_Id).NotEqual(Guid.Empty).
                WithMessage("Client id most be specified to save an invoice");
        }
    }
}
