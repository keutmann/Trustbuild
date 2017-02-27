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
                
                Package = new PackageState();
                var json = db.KeyValue.Get("state");
                if(json != null)
                    Package = JsonConvert.DeserializeObject<PackageState>(json);

                if (Package.Status == WorkflowStatus.Ready)
                    Package.Status = WorkflowStatus.Running;

                if (string.IsNullOrEmpty(Package.Filename))
                    Package.Filename = filename;

                if (Package.WorkflowQueue.Count == 0)
                    Enqueue(typeof(ServerSignWorkflow));

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
