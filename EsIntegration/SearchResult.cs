using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public class SearchResult
    {
        public long Total { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public List<Customer> Results { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public List<Aggregation> aggregations{ get; set; }

    }
}
