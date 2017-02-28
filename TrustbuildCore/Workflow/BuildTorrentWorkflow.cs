using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class BuildTorrentWorkflow : WorkflowBase
    {
        public override void Execute()
        {
            // Build torrent file
            // Simulate work by spinning
            Context.RandomWork();
            Context.Log("Package torrent created");
            Context.Enqueue(typeof(PublishPackageWorkflow));
            Context.Update();
        }
    }
}
