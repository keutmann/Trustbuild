using System.IO;
using TrustbuildCore.Service;
using TrustchainCore.Extensions;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class MovePackageWorkflow : WorkflowPackage
    {
        
        public override void Execute()
        {
            // Move to library
            var librarypath = App.Config["librarypath"].ToStringValue();

            File.Move(Package.Filename, librarypath);

            Context.Enqueue(typeof(SuccessWorkflow));
        }

    }
}
