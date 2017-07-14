using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakHappy.Core.Validation
{
    public interface IValidator
    {
        ValidatorResult Validate(Data.Model.IEntity entity);
    }
}
