using Newtonsoft.Json;
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
            using (var db = TrustchainDatabase.Open(filename))
            {
                var package = new PackageState();
                var json = db.KeyValue.Get("state");
                if(json != null)
                    package = JsonConvert.DeserializeObject<PackageState>(json);

                if (package.Status == WorkflowStatus.Ready)
                    package.Status = WorkflowStatus.Running;

                if (string.IsNullOrEmpty(package.Filename))
                    package.Filename = filename;

                if(string.IsNullOrEmpty(package.ExecutingWorkflowName))
                    package.ExecutingWorkflowName = typeof(TimeStampWorkflow).FullName;

                // Get State from file!
                Push(package.ExecutingWorkflowName);

                Update();
            }

        }

        public override void Log(string message)
        {
            base.Log(message);
            Console.WriteLine(message);
        }

        public override void Update()
        {
            using (var db = TrustchainDatabase.Open(Package.Filename))
            {
                db.KeyValue.Put("state", JsonConvert.SerializeObject(Package));
            }

            }
        }
}
