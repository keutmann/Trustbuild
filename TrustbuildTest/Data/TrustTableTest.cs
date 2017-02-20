using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildTest.Resources;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Service;

namespace TrustbuildTest.Data
{
    [TestFixture]
    public class TrustTableTest 
    {
        [Test]
        public void TestAddDefault()
        {
            using (var db = TrustchainDatabase.Open())
            {
                var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);

                db.Trust.Add(trust);

                var result = db.Trust.Select(trust.Issuer.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual(trust.Signature.Issuer, result.Signature.Issuer);
                Assert.AreEqual(trust.Signature.Server, result.Signature.Server);
                Assert.AreEqual(trust.Server.Id, result.Server.Id);
                Assert.AreEqual(trust.Timestamp.Count(), result.Timestamp.Count());
            }
        }

        [Test]
        public void TestAddBlob()
        {
            int numberOfTestItems = 200;
            using (var db = TrustchainDatabase.Open())
            {
                var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);
                var ids = new List<Trust>();
                using (var timer = new TimeMe("Adding Trust"))
                {
                    for (int i = 0; i < numberOfTestItems; i++)
                    {
                        var id = Guid.NewGuid().ToByteArray();
                        trust.Issuer.Id = id;
                        ids.Add(trust);
                        db.Trust.Add(trust);
                    }
                }

                var count = 0;
                using (var timer = new TimeMe("Reading Trust"))
                {
                    foreach (var item in ids)
                    {
                        var test = db.Trust.Select(item.Issuer.Id);
                        Assert.IsNotNull(test, "count: "+count);
                    }
                }
                Console.WriteLine("MemoryUsed: " + db.Connection.MemoryUsed);

            }
        }
    }
}
