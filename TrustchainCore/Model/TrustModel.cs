using Newtonsoft.Json;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TrustModel
    {
        [JsonIgnore]
        public int RowID { get; set; }

        [JsonProperty(PropertyName = "head")]
        public HeadModel Head { get; set; }

        [JsonProperty(PropertyName = "issuer")]
        public IssuerModel Issuer { get; set; }

        [JsonProperty(PropertyName = "server")]
        public ServerModel Server { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public TimestampModel[] Timestamp { get; set; }
    }
}
