using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class TimeStampWorkflow : WorkflowBase
    {
        public override void Execute()
       {
            Context.Log("Timestamp of trust started");
            Context.Update();
            Context.RandomWork();

            Context.Log("Timestamp of trust done");
            Context.Push(new FinalizePackageWorkflow());
            Context.Update();
        }
    }
}
