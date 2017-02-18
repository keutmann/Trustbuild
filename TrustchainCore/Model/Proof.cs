using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Proof
    {
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "sig")]
        public byte[] Sig { get; set; }

    }
}
