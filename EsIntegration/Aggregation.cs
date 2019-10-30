using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public class Aggregation
    {
        public string aggregationName { get; set; }
        public string aggregationField { get; set; }
        public int docCount { get; set; }
        public List<FilterData> filters { get; set; }
    }

    public class FilterData
    {
        public string key { get; set; }
        public int count { get; set; }
    }
}
