using Nest;

namespace ESDemo.EsIntegration
{
    public class CustomerModel 
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Landline { get; set; }
        public string Email { get; set; }
        public string status { get; set; }
        public School school { get; set; }
        public string EmailSuggest { get; set; }
        public string FirstNameSuggest { get; set; }
        public string LastNameSuggest { get; set; }

        public TypeMappingDescriptor<CustomerModel> GetMapper()
        {
            TypeMappingDescriptor<CustomerModel> mapper = new TypeMappingDescriptor<CustomerModel>();
            return mapper.Properties(
                ps => ps.Text(t =>
                            t.Name(n => n.CustomerId)
                        )
                      .Text(t =>
                            t.Name(n => n.FirstName).Analyzer("standard")
                            )
                      .Text(t =>
                            t.Name(n => n.LastName).Analyzer("standard")
                            )
                      .Text(t =>
                            t.Name(n => n.Email).Fields(fs => fs.Keyword(k => k.Name(n => n.Email))
                            )

                );
            //return mapper.AutoMap();
        }
    }
}
