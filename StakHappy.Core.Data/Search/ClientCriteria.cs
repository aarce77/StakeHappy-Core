using System;

namespace StakHappy.Core.Data.Search
{
    /// <summary>
    /// Search criteria object
    /// </summary>
    public sealed class ClientCriteria : SearchCriteria
    {
        public String CompanyName { get; set; }
    }
}