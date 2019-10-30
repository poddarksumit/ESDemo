

using Nest;

namespace ESDemo.EsIntegration
{
    [ElasticsearchType(RelationName = "school")]
    public class School
    {
        public int schoolid { get; set; }
        [Text(Name = "schooladdress1")]
        public string schooladdress1 { get; set; }
        [Text(Name = "schoolname", Index = true)]
        public string schoolname { get; set; }
        [Keyword(Index = true)]
        public string suburb { get; set; }
        [Text(Name = "postcode", Index = true)]
        public string postcode { get; set; }
        [Keyword(Name = "state", Index = true)]
        public string state { get; set; }
    }
}
