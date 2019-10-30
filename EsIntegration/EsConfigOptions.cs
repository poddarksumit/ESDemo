using BrandLink.Extensions.ElasticSearch.Config;

namespace ESDemo.EsIntegration
{
    public class EsConfigOptions : ConfigBase
    {
        public bool BuildIndexAndMappingOnStartUp { get; set; }
    }
}
