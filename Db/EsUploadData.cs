using BrandLink.Extensions.ElasticSearch.Operations.Data;
using Nest;
using System;

namespace ESDemo.Db
{
    public class EsUploadData : DataOperations
    {


        public EsUploadData(IElasticClient EsClient) : base(EsClient) { }



        public override int GetRecordsCount()
        {
            throw new NotImplementedException();
        }
    }
}
