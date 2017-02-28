using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class FinalizePackageWorkflow : WorkflowPackage
    {
        
        public override void Execute()
        {
            // Compact file
            Context.Log(string.Format("Package {0} has been compacted", ((PackageContext)Context).Filename));


            //Package.Filename
            Context.RandomWork();
            // Move to library
            Context.Log("Package has been moved to library");
            Context.Enqueue(typeof(BuildTorrentWorkflow));
            Context.Update();
        }

    }
}
