using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public class AutoCompleteResponse
    {
        public string text { get; set; }
        public List<CustomerDetails> customerDetails { get; set; }
    }

    public class CustomerDetails
    {
        public string Email { get; set; }
        public Customer Details { get; set; }
    }
}
