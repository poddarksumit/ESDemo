using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public class CustomerAutoComplete
    {
        public IEnumerable<CustomerSuggets> Suggests { get; set; }
    }

    public class CustomerSuggets{
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Landline { get; set; }
        public string Email { get; set; }
        public double Score { get; set; }
    }

}
