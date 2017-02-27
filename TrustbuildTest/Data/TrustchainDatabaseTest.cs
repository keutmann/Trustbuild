using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using TrustbuildTest.Resources;
using TrustchainCore.Business;
using TrustchainCore.Data;

namespace TrustbuildTest.Data
{
    [TestFixture]
    public class TrustchainDatabaseTest
    {
        [Test]
        public void TestAddTrust()
        {
            using (var db = TrustchainDatabase.Open())
            {
                var trust = TrustManager.Deserialize(TrustSimple.JSON);

                db.AddTrust(trust);

                var result = db.Trust.SelectOne(trust.TrustId);

                Assert.IsNotNull(result);
                Assert.AreEqual(trust.Issuer.Signature, result.Issuer.Signature);
                Assert.AreEqual(trust.Server.Signature, result.Server.Signature);
                Assert.AreEqual(trust.Server.Id, result.Server.Id);
                Assert.AreEqual(trust.Timestamp.Count(), result.Timestamp.Count());
                Console.WriteLine("----- Source ---");
                Console.WriteLine(JsonConvert.SerializeObject(trust, Formatting.Indented));
                Console.WriteLine("----- Result ---");
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

        }
    }
}
