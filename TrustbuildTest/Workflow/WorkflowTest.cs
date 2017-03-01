using NBitcoin;
using NBitcoin.Crypto;
using NUnit.Framework;
using TrustbuildCore.Business;
using TrustbuildCore.Workflow;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Extensions;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TrustbuildCore.Service;
using TrustchainCore.Business;

namespace TrustbuildTest.Workflow
{
    [TestFixture]
    public class WorkflowTest
    {
        public TrustModel CreateATrust(string issuerName, string subjectName)
        {
            var issuerKey = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes(issuerName)));
            var subjectKey = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes(subjectName)));

            var trust = new TrustModel();
            trust.Head = new HeadModel
            {
                Version = "standard 0.1.0",
                Script = "btc-pkh"
            };
            trust.Server = new ServerModel();
            trust.Server.Id = ServerIdentity.Current.Address.ToBytes();
            trust.Issuer = new IssuerModel();
            trust.Issuer.Id = issuerKey.PubKey.GetAddress(App.BitcoinNetwork).Hash.ToBytes();
            var subjects = new List<SubjectModel>();
            subjects.Add(new SubjectModel {
                Id = subjectKey.PubKey.GetAddress(App.BitcoinNetwork).Hash.ToBytes(),
                IdType = "person",
                Claim = new JObject(
                    new JProperty("trust", "true")
                    ),
                Scope = "global"
            });
            trust.Issuer.Subjects = subjects.ToArray();

            var binary = new TrustBinary(trust);
            trust.TrustId = TrustECDSASignature.GetHashOfBinary(binary.GetIssuerBinary());
            var trustHash = new uint256(trust.TrustId);
            trust.Issuer.Signature = issuerKey.SignCompact(trustHash);

            return trust;
        }

        [Test]
        public void ServerSignWorkflowTest()
        {
            var serverKey = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("server")));
            App.Config["serverwif"] = serverKey.GetBitcoinSecret(App.BitcoinNetwork).ToWif();
            var manager = new TrustBuildManager();
            var trust = CreateATrust("issuer", "subject");
            var json = JsonConvert.SerializeObject(trust);

            trust = manager.AddNew(json);

            // Execute
            var wf = new ServerSignWorkflow();
            wf.Context = new PackageContext();

            Assert.IsTrue(wf.Initialize());

            wf.Execute();

            // Verify
            using (var db = TrustchainDatabase.Open()) // Using in memory db.
            {
                var item = db.Trust.SelectOne(trust.TrustId);
                Assert.NotNull(item);
                Assert.NotNull(item.Server.Signature);
                Assert.IsTrue(item.Server.Signature.Length > 0);
            }

        }




    }
}
