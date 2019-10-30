using System;
using Nest;

namespace ESDemo.EsIntegration
{
    [ElasticsearchType(RelationName = "customer")]
    public class Customer
    {
        public int Id { get; set; }
        [Text(Name = "customerId", Norms = false, Index = false)]
        public string CustomerId { get; set; }
        [Text(Name = "first_name", Norms = false)]
        public string FirstName { get; set; }
        [Text(Name = "last_name")]
        public string LastName { get; set; }
        [Text(Name = "landline")]
        public string Landline { get; set; }
        [Keyword]        
        public string Email { get; set; }
        [Text(Name = "status")]
        public string status { get; set; }
        [Nested(Enabled =true, IncludeInParent = true, Name = "school")]
        public School school { get; set; }

        [Completion]
        public string EmailSuggest { get; set; }

        [Completion]
        public string FirstNameSuggest { get; set; }

        [Completion]
        public string LastNameSuggest { get; set; }

        public TypeMappingDescriptor<Customer> GetMapper()
        {
            TypeMappingDescriptor<Customer> mapper = new TypeMappingDescriptor<Customer>();
            return mapper.Properties(ps => ps.Text(t => t.Name(n => n.Email).Fields(ff => ff.Text(tt => tt.Name("emailanalyzer").Analyzer("custom_email_analyzer")))));
        }

    }
}
