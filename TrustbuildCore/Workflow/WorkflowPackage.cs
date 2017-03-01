using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Data;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class WorkflowPackage : WorkflowBase
    {
        public PackageContext Package
        {
            get
            {
                return (PackageContext)Context;
            }
            set
            {
                Context= value;
            }
        }
    }
}
