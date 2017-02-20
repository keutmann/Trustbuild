using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Subject
    {
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "id")]
        public byte[] Id { get; set; }

        [JsonProperty(PropertyName = "idtype")]
        public string IdType { get; set; }

        [JsonProperty(PropertyName = "claim")]
        public Claim[] Claims { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "activate")]
        public DateTime Activate { get; set; }

        [JsonProperty(PropertyName = "expire")]
        public DateTime Expire { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Non serializeable
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Non serializeable
        /// </summary>
        public byte[] IssuerId { get; set; }

        public Subject()
        {
            Index = -1;
        }
    }
}
