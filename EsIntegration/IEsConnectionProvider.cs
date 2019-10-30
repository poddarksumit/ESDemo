using BrandLink.Extensions.ElasticSearch.Operations.Model;
using Nest;
using System.Threading.Tasks;

namespace ESDemo.EsIntegration
{
    public interface IEsConnectionProvider
    {
        IEsConfig EsConfig { get; set; }
        bool CreateIndex();
        bool UploadData();
        SearchResult<Customer> GetCustomer(CustomerSearchRequest searchRequest);
        AutoCompleteResponse SuggestCustomer(string query, int page, int pageSize);

    }
}
