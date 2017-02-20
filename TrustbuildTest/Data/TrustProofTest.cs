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
using TrustbuildTest.Resources;

namespace TrustbuildTest.Data
{
    [TestFixture]
    public class TrustProofTest
    {



        [Test]
        public void LoadTrust()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
            Assert.IsTrue(trust != null);
            Assert.IsTrue(trust.Issuer != null);
            Assert.IsTrue(trust.Issuer.Subjects != null);
            Assert.IsTrue(trust.Issuer.Subjects.Length > 0);
        }


        [Test]
        public void GetTrustIssuerHash()
        {
            var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
            Assert.IsTrue(trust != null);

            var trustHash = new TrustHash(trust);

            var data = trustHash.GetIssuerBinary();
            Assert.IsTrue(data.Length > 0);

            var hash = trustHash.GetIssuerHash();
            Assert.IsTrue(hash.Length > 0);
            Console.WriteLine(hash.ConvertToHex());
        }
    }
}
