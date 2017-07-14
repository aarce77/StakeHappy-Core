using System.Collections.Generic;

namespace StakHappy.Core.Validation
{
    public class ValidatorResult
    {
        public bool IsValid { get; set; }
        public IList<ValidatorFailure> Errors { get; set; }

        public ValidatorResult(bool isValid)
        {
            IsValid = isValid;
            Errors = new List<ValidatorFailure>();
        }
    }
    public class ValidatorFailure
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}
