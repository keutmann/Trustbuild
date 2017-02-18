using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Signature
    {
        [JsonProperty(PropertyName = "server")]
        public byte[] Server { get; set; }

        [JsonProperty(PropertyName = "issuer")]
        public byte[] Issuer { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public Proof[] Subject { get; set; }

    }
}
