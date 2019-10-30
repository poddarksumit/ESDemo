using BrandLink.Extensions.ElasticSearch.Core;
using BrandLink.Extensions.ElasticSearch.Extensions;
using BrandLink.Extensions.ElasticSearch.Operations.Configuration;
using BrandLink.Extensions.ElasticSearch.Operations.Core.Interface;
using BrandLink.Extensions.ElasticSearch.Operations.Search.Interface;
using BrandLink.Extensions.ElasticSearch.Operations.Set;
using ESDemo.EsIntegration;
using Microsoft.Extensions.Options;
using Nest;

namespace ESDemo.Search
{
    public class SearchConfiguration : SearchContextBase<EsConfigOptions>, ISearchConfiguration
    {
        // Search Sets
        public SearchSet<CustomerModel> CustomerModelSet { get; set; }
        public SearchSet<Customer> CustomerSet { get; set; }
        public SearchConfiguration(IElasticClient elasticClient, ICoreOperations esCoreOperation, ISearchOperations esSearchOperation, IOptions<EsConfigOptions> options, SearchBuilder searchBuilder) :
                base(elasticClient, esCoreOperation, esSearchOperation, options, searchBuilder)
        {
            var mappingRequrest = new PutMappingRequest("customer");
            mappingRequrest.Properties = SearchBuilder.Entity<CustomerModel>().Descriptor;


            elasticClient.Map(mappingRequrest);
        }



        public override void AddFieldSearchDescriptor()
        {
            CustomerSet.SearchFields.Field(p => p.CustomerId).
                                        Field(p => p.Email).
                                        Field(p => p.FirstName).
                                        Field(p => p.LastName).
                                        Field(p => p.school.schoolname);
            CustomerModelSet.SearchFields.Field(p => p.Email);
        }

        public override void AddAggregationFieldsDescriptor()
        {
            CustomerSet.AggregationFields.Terms(a => a.school.suburb).Terms(a => a.Email);
        }


        public override void ConfigureSearchBuilder()
        {
            SearchBuilder.Entity<CustomerModel>().Descriptor.Properties(
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

        public void ConfigureSearch()
        {
            throw new System.NotImplementedException();
        }
    }
}



public abstract class SearchContextBase
{
    protected SearchContextBase()
    {

        // Use fastmember to look through the properties of this instance and create the descriptors.
        // Not type descriptors?? Throw!!

        CreateIndex();
    }

    public abstract void RegisterEntities();


    public abstract void OnSave<T>(T entity);

    public void CreateIndex()
    {

        // In here you trigger the index creation code.
    }
}


public class AppSearchContext : SearchContextBase
{

    public TypeMappingDescriptor<CustomerModel> CustomerDescriptor { get; set; }

    public override void OnSave<T>(T entity)
    {
        // This can noe be called from the save event in EF Core passing the entity type. 
        throw new System.NotImplementedException();
    }

    public override void RegisterEntities()
    {

        //this.CustomerDescriptor = new TypeMappingDescriptor<CustomerModel>();

        this.CustomerDescriptor.Properties(
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




        throw new System.NotImplementedException();
    }
}