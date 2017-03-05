using NBitcoin;
using NBitcoin.Crypto;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildCore.Business;
using TrustbuildCore.Service;
using TrustbuildCore.Workflow;
using TrustchainCore.Business;
using TrustchainCore.Data;
using TrustchainCore.Model;

namespace TrustbuildTest.Workflow
{
    [TestFixture]
    public class WorkflowPackageTest
    {
        [SetUp]
        public void Setup()
        {
            App.Config["test"] = false;
        }

        [TearDown]
        public void Teardown()
        {
            App.Config["test"] = true;
        }

        //[Test]
        //public void VacuumPackageWorkflow()
        //{
        //    // Setup
        //    var manager = new TrustBuildManager();
        //    manager.Timestamp = new DateTime(2017, 2, 3);
        //    var trust1 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer1", "subject1")));
        //    var trust2 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer2", "subject2")));
        //    var trust3 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer3", "subject3")));
        //    // Execute
        //    var wf = new VacuumPackageWorkflow();
        //    wf.Package = new PackageContext();
        //    wf.Package.Filename = Path.Combine(AppDirectory.BuildPath, trust1.DatabaseName);
        //    var sizeBefore = new FileInfo(wf.Package.Filename).Length;
        //    using (var db = TrustchainDatabase.Open(wf.Package.Filename)) // Using in memory db.
        //    {
        //        db.Trust.DropTable(); // Delete something!
        //    }

        //    Assert.IsTrue(wf.Initialize());
        //    wf.Execute();

        //    var sizeAfter = new FileInfo(wf.Package.Filename).Length;

        //    // Verify
        //    Assert.IsTrue(sizeBefore > sizeAfter);
        //}

        [Test]
        public void MovePackageWorkflow()
        {
            var sourceName = "Test.trust";
            var sourceFile = Path.Combine(AppDirectory.BuildPath, sourceName);
            File.WriteAllText(sourceFile, "Hello world!");

            var movewf = new MovePackageWorkflow();
            movewf.Package = new PackageContext();
            movewf.Package.Filename = sourceName;
            movewf.Package.FilePath = sourceFile;

            // Clean up first
            var librarypath = Path.Combine(AppDirectory.LibraryPath, movewf.Package.Filename);
            if (File.Exists(librarypath))
                File.Delete(librarypath);

            movewf.Execute();

            Assert.IsTrue(File.Exists(movewf.Package.FilePath));
        }

        [Test]
        public void BuildTorrentWorkflow()
        {
            // Setup
            var manager = new TrustBuildManager();
            manager.Timestamp = new DateTime(2017, 2, 4);
            var trust1 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer1", "subject1")));
            var trust2 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer2", "subject2")));
            var trust3 = manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer3", "subject3")));

            // Execute
            var wf = new BuildTorrentWorkflow();
            wf.Package = new PackageContext();
            wf.Package.Filename = trust1.DatabaseName;
            wf.Package.FilePath = Path.Combine(AppDirectory.BuildPath, trust1.DatabaseName);

            Assert.IsTrue(wf.Initialize());
            wf.Execute();

            // Verify

            var torrentFile = Path.Combine(AppDirectory.TorrentPath,  wf.Package.Filename.Replace(".trust", ".torrent"));
            Assert.IsTrue(File.Exists(torrentFile));
        }


        [Test]
        public void PerformanceWorkflowTest()
        {
            // Setup
            var manager = new TrustBuildManager();
            manager.Timestamp = new DateTime(2017, 2, 4);

            var length = 1000;
            var list = new List<TrustModel>();
            var tasks = new List<Task>();

            var dbPath = Path.Combine(AppDirectory.BuildPath, manager.GetCurrentDBTrustname(ServerIdentity.Current.Address.ToBytes()));
            if (File.Exists(dbPath))
                File.Delete(dbPath);

            //for (int i = 0; i < length; i++)
            //{
            //    var t = Task.Run(() =>
            //    {
            //        list.Add(manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer" + i, "subject" + i))));
            //    });

            //    t.Wait();
            //}

            //Task.WaitAll(tasks.ToArray());


            Parallel.For(0, length, (i) =>
            {
                list.Add(manager.AddNew(JsonConvert.SerializeObject(WorkflowTest.CreateATrust("issuer" + i, "subject" + i))));
            });



            // Verify
            using (var db = TrustchainDatabase.Open(dbPath)) // Using in memory db.
            {
                var trusts = db.Trust.Select();
                Console.WriteLine("Trusts : "+trusts.Count());
                Console.WriteLine("List : " + list.Count());

                Assert.AreEqual(trusts.Count(), list.Count);
            }
        }
    }
}
