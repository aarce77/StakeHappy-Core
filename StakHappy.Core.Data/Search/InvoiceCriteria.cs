using System;

namespace StakHappy.Core.Data.Search
{
    public class InvoiceCriteria : SearchCriteria
    {
        public InvoiceCriteria()
        {
            InvoiceDateRange = new DateTimeRange
            {
                From = (DateTime) System.Data.SqlTypes.SqlDateTime.MinValue,
                To = (DateTime) System.Data.SqlTypes.SqlDateTime.MaxValue
            };
            VoidedDateRange = new DateTimeRange { From = default(DateTime), To = default(DateTime) };
            HasBalanceOnly = true;
        }

        public Guid ClientId { get; set; }
        public DateTimeRange InvoiceDateRange { get; set; }
        public String Number { get; set; }
        public bool? Voided { get; set; }
        public DateTimeRange VoidedDateRange { get; set; }
        public bool HasBalanceOnly { get; set; }
        public SortFields SortBy { get; set; }

        public enum SortFields
        {
            Date = 0,
            Number = 1,
            Client = 2
        }
    }
}