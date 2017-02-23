using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustbuildCore.Workflow;

namespace TrustbuildTest.Workflow
{
    [TestFixture]
    public class WorkflowContextTest
    {
        [Test]
        public void TestEngine()
        {
            var list = new List<string>() { "test.db", "test2.dk" };

            var engine = new PackageEngine(list);
            engine.Execute();

            Assert.IsTrue(true);
        }
    }
}
