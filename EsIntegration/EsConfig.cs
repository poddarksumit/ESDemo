using System;

namespace ESDemo.EsIntegration
{
    public class EsConfig : IEsConfig
    {
        string IEsConfig.EsUrl { get => "http://localhost:9200"; set { } }
        string IEsConfig.EsDefault { get => "customer"; set { } }
    }
}
