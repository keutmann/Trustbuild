using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Data;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class VacuumPackageWorkflow : WorkflowPackage
    {
        
        public override void Execute()
        {
            // Remove indexs?

            // Compact file
            GC.Collect(); // Make sure that the file is not locked

            using (var db = TrustchainDatabase.Open(Package.FilePath))
            {
                db.Vacuum();
            }
            Context.Log(string.Format("Package {0} has been compacted", Package.Filename));
            Context.Enqueue(typeof(BuildTorrentWorkflow));
        }

    }
}
