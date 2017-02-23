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
    public class SubjectTableTest 
    {
        [Test]
        public void TestAddDefault()
        {
            using (var db = TrustchainDatabase.Open())
            {
                var trust = JsonConvert.DeserializeObject<TrustModel>(TrustSimple.JSON);

                foreach (var subject in trust.Issuer.Subjects)
                {
                    subject.IssuerId = trust.Issuer.Id;
                    var addResult = db.Subject.Add(subject);
                    Assert.IsTrue(addResult > 0, "Subject was not added!");
                }

                var result = db.Subject.Select(trust.Issuer.Id);
                Assert.IsNotNull(result);

                Assert.AreEqual(trust.Issuer.Subjects.Count(), result.Count());
                var subjectResult = result.ToList();
                var subjectIndex = 0;
                foreach (var subject in trust.Issuer.Subjects)
                {
                    Assert.AreEqual(subject.Id, subjectResult[subjectIndex++].Id);
                }
            }
        }
    }
}
