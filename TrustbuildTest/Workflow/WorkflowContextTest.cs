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
            var engine = new PackageEngine();
            engine.Load();
            engine.Execute();

            Assert.IsTrue(true);
        }
    }
}
