using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakHappy.Core.Data.Model
{
    public partial class PaymentType
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public String Description { get; set; }
        public String Label { get; set; }
    }
}
