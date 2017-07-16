using System;

namespace StakHappy.Core.Data.Search
{
    public interface ISearchCriteria
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
        int Skip { get; }
        SortDirection SortOrder { get; set; }
        Guid UserId { get; set; }
    }
}