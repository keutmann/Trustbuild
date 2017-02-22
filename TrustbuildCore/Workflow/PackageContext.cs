using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class PackageContext : WorkflowContext
    {

        public PackageState Package
        {
            get
            {
                return (PackageState)State;
            }
            set
            {
                State = value;
            }
        }

        public PackageContext(string filename)
        {
            // Load db file
            Package = new PackageState();
            Package.Filename = filename;
            // Get State from file!
            Push(new TimeStampWorkflow());
        }

        public override void Log(string message)
        {
            base.Log(message);
            Console.WriteLine(message);
        }
    }
}
