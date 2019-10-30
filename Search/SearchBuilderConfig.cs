using BrandLink.Extensions.ElasticSearch.Operations.Configuration;
using ESDemo.EsIntegration;

namespace ESDemo.Search
{
    public class SearchBuilderConfig
    {
        public SearchBuilder SearchBuilder;

        public SearchBuilderConfig(SearchBuilder searchBuilder)
        {
            SearchBuilder = searchBuilder;
        }
        public void BuildSearchBuilder()
        {            
            SearchBuilder.Entity<EsIntegration.CustomerModel>().Descriptor.Properties(
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
                                        t.Name(n => n.Email).Fields(fs => fs.Keyword(k => k.Name("emailRaw")))
                                        )

                            );
            SearchBuilder.Entity<Customer>().Descriptor.Properties(
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
                            t.Name(n => n.Email).Fields(fs => fs.Keyword(k => k.Name("emailRaw")))
                            )

                );
        }
    }
}
