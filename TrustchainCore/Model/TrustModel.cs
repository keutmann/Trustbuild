using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TrustModel
    {
        [JsonIgnore]
        public byte[] TrustId { get; set; }

        [JsonProperty(PropertyName = "head")]
        public HeadModel Head { get; set; }

        [JsonProperty(PropertyName = "issuer")]
        public IssuerModel Issuer { get; set; }

        [JsonProperty(PropertyName = "server")]
        public ServerModel Server { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public Dictionary<string,TimestampModel> Timestamp { get; set; }

        public TrustModel()
        {
            Timestamp = new Dictionary<string, TimestampModel>();
        }
    }
}
