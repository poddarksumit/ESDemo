using BrandLink.Extensions.ElasticSearch.Operations.Model;
using ESDemo.EsIntegration;
using Microsoft.AspNetCore.Mvc;

namespace ESDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EsSearchController : ControllerBase
    {
        public IEsConnectionProvider EsConnectionProvider;

        public EsSearchController(IEsConnectionProvider esConnectionProvider)
        {
            EsConnectionProvider = esConnectionProvider;
        }

        // GET api/values
        [HttpGet]
        [Route("createIndex")]
        public ActionResult<bool> CreateIndex()
        {
            return EsConnectionProvider.CreateIndex();
        }

        // GET api/values
        [HttpPost]
        //[Route("search}")]
        public ActionResult<SearchResult<Customer>> Search([FromBody] CustomerSearchRequest searchRequest)
        {
            return EsConnectionProvider.GetCustomer(searchRequest);
        }


        [HttpGet]
        [Route("suggest/{searchTerm}")]
        public ActionResult<AutoCompleteResponse> Suggest(string searchTerm)
        {
            return EsConnectionProvider.SuggestCustomer(searchTerm, 0, 10);
        }

        // POST api/values
        [HttpGet]
        public void UploadData()
        {
            EsConnectionProvider.UploadData();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
