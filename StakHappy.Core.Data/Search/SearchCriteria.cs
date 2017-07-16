using System;

namespace StakHappy.Core.Data.Search
{
    public class SearchCriteria : ISearchCriteria
    {
        public int? PageSize { get; set; }
        public int? Page { get; set; }

        public int Skip
        {
            get
            {
                if (!Page.HasValue || !PageSize.HasValue)
                    return 0;
                if (Page.Value > 1)
                    return (Page.Value - 1) * PageSize.Value;
                return 0;
            }
        }
        public SortDirection SortOrder { get; set; }
        public Guid UserId { get; set; }

        protected SearchCriteria()
        {
            PageSize = 25;
            SortOrder = SortDirection.ASC;
        } 
    }
}