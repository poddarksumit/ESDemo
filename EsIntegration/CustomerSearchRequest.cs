using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public class CustomerSearchRequest
    {
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public string searchTerm { get; set; }
        public List<Filter> filters { get; set; }

    }

    public class Filter
    {
        public string fieldName { get; set; }
        public string fieldValue { get; set; }
    }
}
