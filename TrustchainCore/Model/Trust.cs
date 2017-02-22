using Newtonsoft.Json;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Trust
    {
        [JsonIgnore]
        public int RowID { get; set; }

        [JsonProperty(PropertyName = "head")]
        public Head Head { get; set; }

        [JsonProperty(PropertyName = "issuer")]
        public Issuer Issuer { get; set; }

        [JsonProperty(PropertyName = "server")]
        public Server Server { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public Timestamp[] Timestamp { get; set; }
    }
}
