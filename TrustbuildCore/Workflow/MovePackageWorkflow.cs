using System;
using System.IO;
using TrustbuildCore.Service;
using TrustchainCore.Business;
using TrustchainCore.Extensions;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class MovePackageWorkflow : WorkflowPackage
    {
        
        public override void Execute()
        {
            // Move to library
            var librarypath = Path.Combine(AppDirectory.LibraryPath, Package.Filename);

            GC.Collect(); // Release old connections, to make sure that the file is not locked
            
            File.Move(Package.FilePath, librarypath);

            Package.FilePath = librarypath;
            Context.Enqueue(typeof(SuccessWorkflow));
        }

    }
}
