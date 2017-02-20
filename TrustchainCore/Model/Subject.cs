﻿using Newtonsoft.Json;
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
        /// <summary>
        /// Subject target id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public byte[] Id { get; set; }

        [JsonProperty(PropertyName = "idtype")]
        public string IdType { get; set; }

        /// <summary>
        /// Not included in the Binary payload for signature verification!
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public byte[] Signature { get; set; }

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
        /// Not included in the Binary payload for signature verification!
        /// </summary>
        public byte[] IssuerId { get; set; }
    }
}
