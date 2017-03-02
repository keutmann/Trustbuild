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
        [SetUp]
        public void Setup()
        {

            var serverKey = new Key(Hashes.SHA256(Encoding.UTF8.GetBytes("server")));
            App.Config["serverwif"] = serverKey.GetBitcoinSecret(App.BitcoinNetwork).ToWif();

            // Using in memory db make sure that tables exist
            using (var db = TrustchainDatabase.Open()) 
            {
                db.Trust.CreateIfNotExist();
                db.KeyValue.CreateIfNotExist();
            }

        }

        [TearDown]
        public void Teardown()
        {
            // Using in memory db make sure that tables are deleted before next test
            using (var db = TrustchainDatabase.Open()) 
            {
                db.Trust.DropTable();
                db.KeyValue.DropTable();
            }
        }

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
            var manager = new TrustBuildManager();
            var trust = CreateATrust("issuer", "subject");
            trust = manager.AddNew(JsonConvert.SerializeObject(trust));

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

        [Test]
        public void BuildBitcoinMerkleWorkflowTest()
        {
            // Setup
            var manager = new TrustBuildManager();
            var trust1 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer1", "subject1")));
            var trust2 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer2", "subject2")));
            var trust3 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer3", "subject3")));

            // Execute
            var wf = new BuildBitcoinMerkleWorkflow();
            wf.Context = new PackageContext();
            Assert.IsTrue(wf.Initialize());
            wf.Execute();

            // Verify
            using (var db = TrustchainDatabase.Open()) // Using in memory db.
            {
                var trusts = db.Trust.Select();
                foreach (var trust in trusts)
                {
                    Assert.NotNull(trust);
                    Assert.NotNull(trust.Timestamp);
                    var btc = trust.Timestamp[wf.Package.TimestampName];

                    Assert.NotNull(btc);
                    Assert.NotNull(btc.Path);
                    Assert.IsTrue(btc.Path.Length > 0);
                    System.Console.WriteLine("Path: "+ btc.Path.ConvertToHex());
                }
            }
        }

        [Test]
        public void TimeStampUpdateWorkflowTest()
        {
            // Setup
            var manager = new TrustBuildManager();
            var trust1 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer1", "subject1")));
            var trust2 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer2", "subject2")));
            var trust3 = manager.AddNew(JsonConvert.SerializeObject(CreateATrust("issuer3", "subject3")));

            // Build up path and root
            var mwf = new BuildBitcoinMerkleWorkflow();
            mwf.Context = new PackageContext();
            mwf.Initialize();
            mwf.Execute();

            // Execute
            var wf = new TimeStampUpdateWorkflow();
            wf.Context = mwf.Context;
            wf.Package.RootPath = wf.Package.RootHash;
            Assert.IsTrue(wf.Initialize());
            wf.Execute();

            // Verify
            using (var db = TrustchainDatabase.Open()) // Using in memory db.
            {
                var trusts = db.Trust.Select();
                foreach (var trust in trusts)
                {
                    Assert.NotNull(trust);
                    Assert.NotNull(trust.Timestamp);
                    var btc = trust.Timestamp[wf.Package.TimestampName];

                    Assert.NotNull(btc);
                    Assert.NotNull(btc.Path);
                    Assert.IsTrue(btc.Path.Length > 0);
                    System.Console.WriteLine("Path: " + btc.Path.ConvertToHex());
                }
            }
        }

        [Test]
        public void BuildTorrentWorkflowTest()
        {
        }
    }
}
