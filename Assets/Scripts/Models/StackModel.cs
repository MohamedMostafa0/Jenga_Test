using System;
using Newtonsoft.Json;

namespace JengaTest.Models
{
    [Serializable]
    public class StackModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("grade")]
        public string Grade { get; set; }
        [JsonProperty("mastery")]
        public int Mastery { get; set; }
        [JsonProperty("domainid")]
        public string Domainid { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("cluster")]
        public string Cluster { get; set; }
        [JsonProperty("standardid")]
        public string StandardId { get; set; }
        [JsonProperty("standarddescription")]
        public string StandardDescription { get; set; }
    }
}
