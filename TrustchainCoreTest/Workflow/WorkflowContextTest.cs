using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustchainCoreTest.Workflow
{
    [TestFixture]
    public class WorkflowContextTest
    {

        public static WorkflowContext TestContext = null;


        static WorkflowContextTest()
        {
            TestContext = new WorkflowContext();
            //TestSate.WF = typeof(SuccessWorkflow);
            TestContext.WorkflowQueue.Enqueue(typeof(SuccessWorkflow));
            TestContext.Status = WorkflowStatus.Finished;
            TestContext.Logs.Add(new WorkflowLog
            {
                Message = "Test",
                Time = DateTime.Now
            });
            TestContext.KeyValue["test"] = "test";
            TestContext.KeyValue["John"] = "Doe";
        }

        [Test]
        public void Serialize()
        {

            var json = JsonConvert.SerializeObject(TestContext);

            Assert.IsTrue(json.Length > 0);
        }

        [Test]
        public void Deserialize()
        {
            var json = JsonConvert.SerializeObject(TestContext);
            var controlState = JsonConvert.DeserializeObject<WorkflowContext>(json);

            //var instance = Activator.CreateInstance(controlState.WF);
            Assert.IsTrue(controlState != null);
            Assert.IsTrue(TestContext.WorkflowQueue.Peek().GetType().FullName == controlState.WorkflowQueue.Peek().GetType().FullName);
            Assert.IsTrue(TestContext.Status == controlState.Status);
            Assert.IsTrue(TestContext.Logs.Count == controlState.Logs.Count);
            Assert.IsTrue(TestContext.KeyValue["test"] == controlState.KeyValue["test"]);
            Assert.IsTrue(TestContext.KeyValue["john"] == controlState.KeyValue["JOHN"]);
        }
    }
}

