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
                var trust = JsonConvert.DeserializeObject<Trust>(TrustSimple.JSON);

                db.AddTrust(trust);

                var result = db.GetTrust(trust.Issuer.Id);

                Assert.IsNotNull(result);
                Assert.AreEqual(trust.Signature.Issuer, result.Signature.Issuer);
                Assert.AreEqual(trust.Signature.Server, result.Signature.Server);
                Assert.AreEqual(trust.Server.Id, result.Server.Id);
                Assert.AreEqual(trust.Timestamp.Count(), result.Timestamp.Count());
                Assert.AreEqual(trust.Issuer.Subjects.Count(), result.Issuer.Subjects.Count());
                Assert.AreEqual(trust.Signature.Subject.Count(), result.Signature.Subject.Count());
                Console.WriteLine("----- Source ---");
                Console.WriteLine(JsonConvert.SerializeObject(trust, Formatting.Indented));
                Console.WriteLine("----- Result ---");
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

        }
    }
}
