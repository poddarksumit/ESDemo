using System;
using System.Collections.Generic;
using BrandLink.Extensions.ElasticSearch.Operations.Core.Interface;
using BrandLink.Extensions.ElasticSearch.Operations.Model;
using BrandLink.Extensions.ElasticSearch.Operations.Model.Request;
using ESDemo.Db;
using ESDemo.Db.Model;
using ESDemo.Search;
using Nest;

namespace ESDemo.EsIntegration
{
    public class EsConnectionProvider : IEsConnectionProvider
    {
       public IEsConfig EsConfig { get; set; }

        public IDictionary<string, string> aggFields = new Dictionary<string, string>()
        {
            { "schoolsuburb", "school.suburb" },
            { "schoolstate", "school.state" },
            { "email1", "email" }
        };

        public EsConnectionProvider(IEsConfig config, ICoreOperations esCoreOperation, SearchConfiguration searchConfiguration)
        {
            EsConfig = config;
            EsCoreOperation = esCoreOperation;
            SearchConfiguration = searchConfiguration;
            BuildEsConnectionManager();
        }

        public  ElasticClient EsClient { get;  }
        public  ICoreOperations EsCoreOperation { get;}
        public  SearchConfiguration SearchConfiguration { get; }

        private void BuildEsConnectionManager()
        {
            if (EsClient == null)
            {
                var connectionString = new ConnectionSettings(new Uri(EsConfig.EsUrl)).DisableDirectStreaming();
                connectionString.DefaultIndex(EsConfig.EsDefault);
                //EsClient = new ElasticClient(connectionString);
            }
        }

        public SearchResult<Customer> GetCustomer(CustomerSearchRequest customerSearchRequest)
        {
            var filters = new List<Func<QueryContainerDescriptor<Customer>, QueryContainer>>();
            if (customerSearchRequest.filters?.Count > 0)
            {
                foreach (Filter filter in customerSearchRequest.filters)
                {
                    filters.Add(fq => fq.Term(t => t.Field(filter.fieldName).Value(filter.fieldValue)));
                }
            }


            AggregationBase agg =
                new TermsAggregation("school.suburb") { Field = Nest.Infer.Field<Customer>(p => p.school.suburb) } &&
                new TermsAggregation("school.suburb") { Field = Nest.Infer.Field<Customer>(p => p.school.state) };

            var response = EsCoreOperation.SearchClient.Search<Customer>(NewMethod2(customerSearchRequest, filters));


            var fileter = new List<BrandLink.Extensions.ElasticSearch.Operations.Model.Request.Filter>();
            foreach (Filter filter in customerSearchRequest.filters)
            {
                fileter.Add(new BrandLink.Extensions.ElasticSearch.Operations.Model.Request.Filter()
                {
                    FieldName = filter.fieldName,
                    FieldValue = filter.fieldValue
                });
            }

            SearchParams searchRequest = new SearchParams()
            {
                SearchTerm = customerSearchRequest.searchTerm,
                Filters = fileter
            };

            var responseNew = SearchConfiguration.CustomerSet.Query(searchRequest);
            
            var responseNew1 = SearchConfiguration.CustomerSet.Query(() => NewMethod3(customerSearchRequest, filters));

            var searchResult = new SearchResult()
            {
                Total = response.Total,
                Page = customerSearchRequest.pageIndex,
                PageSize = customerSearchRequest.pageSize,
                Results = (List<Customer>)response.Documents
            };

            searchResult.aggregations = BuildAggregations(response.Aggregations);


            return responseNew;
        }

        private static Func<SearchDescriptor<Customer>, ISearchRequest> NewMethod2(CustomerSearchRequest customerSearchRequest, List<Func<QueryContainerDescriptor<Customer>, QueryContainer>> filters)
        {
            return s => s.Index("customer").
                            Query(q =>
                                q.Bool(b =>
                                   b.Must(mu =>
                                          mu.MultiMatch(m => m.Fields(f => NewMethod1(f)).Query(customerSearchRequest.searchTerm))
                                       ).Filter(filters)
                                    )
                                ).Aggregations(a => NewMethod()).Skip(customerSearchRequest.pageIndex).Take(20);
        }

        private static SearchDescriptor<Customer> NewMethod3(CustomerSearchRequest customerSearchRequest, List<Func<QueryContainerDescriptor<Customer>, QueryContainer>> filters)
        {
            SearchDescriptor<Customer> sd = new SearchDescriptor<Customer>().Index("customer").
                            Query(q =>
                                q.Bool(b =>
                                   b.Must(mu =>
                                          mu.MultiMatch(m => m.Fields(f => NewMethod1(f)).Query(customerSearchRequest.searchTerm))
                                       ).Filter(filters)
                                    )
                                ).Aggregations(a => NewMethod(a)).Skip(customerSearchRequest.pageIndex).Take(20);


            return sd;
        }

        private static AggregationContainerDescriptor<Customer> NewMethod(AggregationContainerDescriptor<Customer> a)
        {
            return a.Terms("school.suburb", t => t.Field(f => f.school.suburb)).Terms("school.suburb", t => t.Field(f => f.school.state));//.Max("maxId", t => t.Field(f => f.CustomerId));
        }

        private static AggregationContainerDescriptor<Customer> NewMethod()
        {
            return new AggregationContainerDescriptor<Customer>().Terms("school.suburb", t => t.Field(f => f.school.suburb)).Terms("school.suburb", t => t.Field(f => f.school.state));//.Max("maxId", t => t.Field(f => f.CustomerId));
        }

        private static FieldsDescriptor<Customer> NewMethod1(FieldsDescriptor<Customer> f)
        {
            return f.Field(p => p.Email).
                                        Field(p => p.FirstName).
                                        Field(p => p.LastName).
                                        Field(p => p.school.schoolname);
        }

        private List<Aggregation> BuildAggregations(AggregateDictionary aggregations)
        {
            if (aggregations.Count == 0)
            {
                return null;
            }
            List<Aggregation> aggResult = new List<Aggregation>();
            foreach(var esAggResult in aggregations)
            {
                aggResult.Add(new Aggregation
                {
                    aggregationName = esAggResult.Key,
                    aggregationField = esAggResult.Key,
                    docCount = ((BucketAggregate)esAggResult.Value).Items.Count,
                    filters = BuildFilters(((BucketAggregate)esAggResult.Value).Items)
                });
            }
            return aggResult;
        }

        private List<FilterData> BuildFilters(IEnumerable<IBucket> values)
        {
            List<FilterData> filterData = new List<FilterData>();
            foreach(KeyedBucket<object> value in values)
            {
                filterData.Add(new FilterData
                {
                    key = (string)value.Key,
                    count = (int)value.DocCount
                });
            }
            return filterData;
        }

        public AutoCompleteResponse SuggestCustomer(string query, int page, int pageSize)
        {
            var response = EsClient.Search<Customer>(s => s.Index("customer")
                .Suggest(su => 
                    su.Completion("suggestions", c => 
                        c.Field(t => 
                            t.EmailSuggest).Prefix(query).Fuzzy(f => 
                                f.Fuzziness(Fuzziness.Auto)
                                )
                            )
                    )
                );
            var suggestions = response.Suggest["suggestions"];

            AutoCompleteResponse autoCompleteResponse = new AutoCompleteResponse
            {
                text = suggestions[0].Text,
                customerDetails = new List<CustomerDetails>()
            };
            foreach(var option in suggestions[0].Options)
            {
                autoCompleteResponse.customerDetails.Add(new CustomerDetails
                {
                    Email = option.Text,
                    Details = option.Source
                });
            }

            return autoCompleteResponse;
        }

        public bool CreateIndex()
        {
            // To Create the index. It doesn't create duplicate.
            // TODO: if any attribute changes or gets added.
            var indexSettings = new IndexSettings();
            var emailAnalyzer = new CustomAnalyzer
            {
                Filter = new List<string> { "lowercase", "uppercase", "asciifolding","stop"},
                Tokenizer = "uax_url_email"                
            };

            var analyzers = new Analyzers();
            analyzers.Add("custom_email_analyzer", emailAnalyzer);
            var indexstate = new IndexState();
            indexstate.Settings = new IndexSettings
            {
                Analysis = new Analysis
                {
                    Analyzers = analyzers
                }
            };
            
            var availa = EsClient.Indices.Exists("customer");
            if (availa.Exists)
            {
                //EsClient.Indices.Delete("customer");
            }
            //EsCoreOperation.CreateIndex(EsClient,"CustomerModel");
            EsCoreOperation.RefereshIndex<CustomerModel>(() => new CustomerModel().GetMapper());
            //.Properties(ps => ps.Text(t => t.Name(n => n.Email).Fields(ff => ff.Text(tt => tt.Name("emailanalyzer").Analyzer("custom_email_analyzer"))))))
            //UploadData();
            //ps.Completion(com => com.Name(p => p.Suggest))
            
            return true;
        }


        public bool UploadData()
        {
            IEsDbOperation esDbOperation = new EsDbOperation();
            int customerTotalCount = esDbOperation.GetCustomersCount();

            int size = 500, index = 0;
            while (index <= customerTotalCount)
            {
                List<KcaCustomer> customers = esDbOperation.GetCustomers(size, index);
                List<Customer> esCustomer = new List<Customer>();

                customers.ForEach(x => esCustomer.Add(new Customer
                {
                    Email = x.Email,
                    EmailSuggest = x.Email,
                    FirstName = x.FirstName,
                    FirstNameSuggest = x.FirstName,
                    LastName = x.LastName,
                    LastNameSuggest = x.LastName,
                    Landline = x.Landline,
                    Id = x.Id,
                    CustomerId = x.Id.ToString(),
                    school = new School
                    {
                        schoolid = x.School.Id,
                        schoolname = x.School.Name,
                        schooladdress1 = x.School.Address,
                        postcode = x.School.Suburb.Postcode.Code,
                        suburb = x.School.Suburb.Name,
                        state = x.School.Suburb.Postcode.State.Name
                    }
                })) ;
                EsClient.BulkAsync(b => b.Index("customer").IndexMany(esCustomer));
                index += size;
            }

            
            return true;
        }
    }
}
