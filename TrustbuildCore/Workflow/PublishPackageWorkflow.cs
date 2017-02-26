using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class PublishPackageWorkflow : WorkflowBase
    {
        public override void Execute()
        {
            // Publish the package here!
            Context.RandomWork();

            Context.Log("Package published");
            Context.Enqueue(new SuccessWorkflow());
            Context.Update();
        }
    }
}
