using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildCore.Business;
using TrustbuildTest.Resources;
using TrustchainCore.Business;
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
                db.Trust.DropTable();
                db.Trust.CreateIfNotExist();

                var trust = TrustManager.Deserialize(TrustSimple.JSON);

                db.Trust.Add(trust);

                var result = db.Trust.SelectOne(trust.TrustId);
                Assert.IsNotNull(result);
                Assert.AreEqual(trust.Issuer.Signature, result.Issuer.Signature);
                Assert.AreEqual(trust.Server.Signature, result.Server.Signature);
                Assert.AreEqual(trust.Server.Id, result.Server.Id);
                Assert.AreEqual(trust.Timestamp.Count(), result.Timestamp.Count());
            }
        }

        [Test]
        public void TestAddBlob()
        {
            int numberOfTestItems = 10;
            using (var db = TrustchainDatabase.Open())
            {
                var trust = TrustManager.Deserialize(TrustSimple.JSON);

                var ids = new List<TrustModel>();
                using (var timer = new TimeMe("Adding Trust"))
                {
                    for (int i = 0; i < numberOfTestItems; i++)
                    {
                        var id = Guid.NewGuid().ToByteArray();
                        trust.Issuer.Id = id;
                        trust.TrustId = TrustManager.GetTrustId(trust);

                        ids.Add(trust);
                        db.Trust.Add(trust);
                    }
                }

                var count = 0;
                using (var timer = new TimeMe("Reading Trust"))
                {
                    foreach (var item in ids)
                    {
                        var test = db.Trust.SelectOne(item.TrustId);
                        Assert.IsNotNull(test, "count: "+count);
                    }
                }
                Trace.TraceInformation("MemoryUsed: " + db.Connection.MemoryUsed);

            }
        }
    }
}
