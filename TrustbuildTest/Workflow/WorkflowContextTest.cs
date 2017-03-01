using NUnit.Framework;
using System.Collections.Generic;
using TrustbuildCore.Workflow;
using TrustchainCore.Workflow;

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
            Assert.IsTrue(engine.Tasks.Count > 0);
            Assert.IsTrue(engine.Tasks[0].Status == WorkflowStatus.Running);
        }
    }
}
