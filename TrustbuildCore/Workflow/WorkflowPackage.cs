using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class WorkflowPackage : WorkflowBase
    {
        public PackageState Package
        {
            get
            {
                return (PackageState)Context.State;
            }
            set
            {
                Context.State = value;
            }
        }
    }
}
