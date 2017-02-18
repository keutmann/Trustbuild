using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Extensions;

namespace TrustbuildTest.Data
{
    [TestFixture]
    public class TrustProofTest
    {
        public static string aTrust = @"
  {
    ""head"": {
    ""version"": ""standard 0.1.0"", 
    ""script"": ""btc-pkh"" 
  },
  ""issuer"": 
    {
      ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"",
      ""subject"": [
        {
          ""index"": 0,
          ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"", 
          ""idtype"": ""person"", 
          ""claimtype"": ""simpletrust"", 
          ""claim"": [{
            ""type"": ""trust"", 
            ""data"": ""true""
              },
              {
                ""type"": ""confirm"", 
                ""data"": ""true""
              }],
          ""cost"": 100,
          ""activate"": ""2017-02-18T23:44:41.7620108+01:00"", 
          ""expire"":   ""2017-02-18T23:44:41+01:00"",
          ""scope"": ""global"" 
        }
      ]
    }
  ,
  ""server"": {
    ""id"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
  },
  ""timestamp"": [
    {
      ""blockchain"": ""bitcoin"",
      ""hash"": ""SHA160"", 
      ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
    },
    {
      ""blockchain"": ""ethereum"",
      ""hash"": ""SHA3"", 
      ""path"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
    }
  ],
  ""signature"": {
    ""server"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"",
    ""issuer"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz"",
    ""subject"": [
        {
        ""index"": 0,
        ""proof"": ""VGhpcyBpcyBhIGN1c3RvbSBhZGRyZXNz""
        }
    ]
  }
}
";


        [Test]
        public void LoadTrust()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(aTrust);
            Assert.IsTrue(trust != null);
            Assert.IsTrue(trust.Issuer != null);
            Assert.IsTrue(trust.Issuer.Subjects != null);
            Assert.IsTrue(trust.Issuer.Subjects.Length > 0);
        }


        [Test]
        public void GetTrustIssuerHash()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(aTrust);
            Assert.IsTrue(trust != null);

            var trustHash = new TrustHash(trust);

            var data = trustHash.GetIssuerBinary();
            Assert.IsTrue(data.Length > 0);

            var hash = trustHash.GetIssuerHash();
            Assert.IsTrue(hash.Length > 0);
            Console.WriteLine(hash.ConvertToHex());
        }



        [Test]
        public void TestJSON()
        {
            var p = new Person
            {
                Name = "Carsten",
                id = Encoding.UTF8.GetBytes("Carsten"),
                Date = DateTime.Now
            };

            var json = JsonConvert.SerializeObject(p, Formatting.Indented);
            Console.WriteLine(json);
            var back = JsonConvert.DeserializeObject<Person>(json);

            Assert.IsTrue(p.Name == back.Name);
            Assert.IsTrue(p.id.SequenceEqual(back.id));

        }

        public static T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
        }


    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Person
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public byte[] id { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }
    }
}
