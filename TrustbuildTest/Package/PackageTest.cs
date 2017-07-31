using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using TrustchainCore.Model;
using TrustchainCore.Service;

namespace TrustbuildTest.Package
{
    [TestFixture]
    public class PackagesTests
    {
        [SetUp]
        public void Setup()
        {
            App.Config["test"] = false; // Build the db file!
        }

        [TearDown]
        public void Teardown()
        {
            App.Config["test"] = true;
        }

        [Test]
        public void BuildChain()
        {

            var manager = new TrustBuildManager();

            var dbPath = Path.Combine(AppDirectory.BuildPath, manager.GetCurrentDBTrustname());
            // Clean up first
            if (File.Exists(dbPath))
                File.Delete(dbPath);

            //manager.Timestamp = new DateTime(2017, 2, 4);
            var trust1 = manager.AddNew(TrustBuilder.CreateTrust("A", "B", TrustBuilder.CreateTrustTrue()));
            var trust2 = manager.AddNew(TrustBuilder.CreateTrust("B", "C", TrustBuilder.CreateTrustTrue()));


            var list = new List<string>() { Path.Combine(AppDirectory.BuildPath, trust1.DatabaseName) };
            var engine = new PackageEngine(list);



            engine.Execute();

            Assert.IsTrue(File.Exists(Path.Combine(AppDirectory.LibraryPath, trust1.DatabaseName)));
            //Assert.IsTrue(engine.Tasks.Count > 0);
            //Assert.IsTrue(engine.Tasks[0].Status == WorkflowStatus.Running);
        }

        [Test]
        public void QueryGraph()
        {
            var url = new Uri("http://localhost:12702/api/query");
            // A -> C
            var body = @"            
                        {
                            ""Issuer"": ""Sbh / W9tWejJQB0IplfwHzgBWa98 = "",
                            ""Subject"": ""BJv2hKA2N4wGZiuDefnJ3Du88Iw="",
                            ""SubjectType"": ""person"",
                            ""Scope"": ""global"",
                            ""Activate"": 0,
                            ""Expire"": 0,
                            ""Claim"": {
                                ""trust"": true
                            }
                        }";

            var service = new WebService();

            var result = service.UploadString(url, body);
            var obj = JToken.Parse(result);
            Console.WriteLine(obj.ToString(Formatting.Indented));
        }
    }
}
