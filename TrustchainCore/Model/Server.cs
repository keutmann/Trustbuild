using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Server
    {
        [JsonProperty(PropertyName = "id")]
        public byte[] Id { get; set; }
    }
}
