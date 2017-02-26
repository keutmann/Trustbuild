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
    public class WorkflowStateTest
    {

        public static WorkflowState TestSate = null;


        static WorkflowStateTest()
        {
            TestSate = new WorkflowState();

            TestSate.WorkflowQueue.Enqueue(new SuccessWorkflow());
            TestSate.Status = WorkflowStatus.Finished;
            TestSate.Log.Add(new WorkflowLog
            {
                Message = "Test",
                Time = DateTime.Now
            });
            TestSate.KeyValue["test"] = "test";
            TestSate.KeyValue["John"] = "Doe";
        }

        [Test]
        public void Serialize()
        {

            var json = JsonConvert.SerializeObject(TestSate);

            Assert.IsTrue(json.Length > 0);
        }

        [Test]
        public void Deserialize()
        {

            var json = JsonConvert.SerializeObject(TestSate);
            var controlState = JsonConvert.DeserializeObject<WorkflowState>(json);
            
            Assert.IsTrue(controlState != null);
            Assert.IsTrue(TestSate.WorkflowQueue.Peek().GetType().FullName == controlState.WorkflowQueue.Peek().GetType().FullName);
            Assert.IsTrue(TestSate.Status == controlState.Status);
            Assert.IsTrue(TestSate.Log.Count == controlState.Log.Count);
            Assert.IsTrue(TestSate.KeyValue["test"] == controlState.KeyValue["test"]);
            Assert.IsTrue(TestSate.KeyValue["john"] == controlState.KeyValue["JOHN"]);
        }
    }
}

