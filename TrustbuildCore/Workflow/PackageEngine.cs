using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class PackageEngine : WorkflowEngine
    {
        public PackageEngine(List<string> packages)
        {
            foreach (var file in packages)
            {
                Tasks.Add(new PackageContext(file));
            }
        }
    }
}
