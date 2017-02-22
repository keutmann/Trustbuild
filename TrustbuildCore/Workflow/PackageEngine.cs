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
        public void Load()
        {
            // Find packages
            var files = new string[] { "test.trust", "test1.trust", "test2.trust", "test4.trust" };

            // 
            foreach (var file in files)
            {
                Tasks.Add(new PackageContext(file));
            }
            
        }
    }
}
