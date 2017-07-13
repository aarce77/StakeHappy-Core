using System;

namespace StakHappy.Core.Data.Search
{
    public class DateTimeRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public DateTime SqlMinValue
        {
            get { return (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue; }
        }

        public DateTime SqlMaxValue
        {
            get { return (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue; }
        } 
    }
}