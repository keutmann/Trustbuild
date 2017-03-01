using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustchainCore.Data;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class PackageContext : WorkflowContext
    {
        public string Filename { get; set; }

        public string TimestampName { get; set; }
        public byte[] RootHash { get; set; }
        public byte[] RootPath { get; set; }

        public int ProofWaitCount { get; set; }


        public PackageContext() : base()
        {
        }

        public static WorkflowContext Create(string filename) 
        {
            var Package = new PackageContext();
            // Load db file
            using (var db = TrustchainDatabase.Open(filename))
            {
                var json = db.KeyValue.Get("state");
                if (json != null)
                    Package = JsonConvert.DeserializeObject<PackageContext>(json);

                if (Package.Status == WorkflowStatus.Ready)
                    Package.Status = WorkflowStatus.Running;

                if (string.IsNullOrEmpty(Package.Filename))
                    Package.Filename = filename;

                if (Package.WorkflowQueue.Count == 0)
                    Package.Enqueue(typeof(ServerSignWorkflow));

                Package.Update();
            }
            return Package;
        }

        public override void Log(string message)
        {
            base.Log(message);
            Console.WriteLine(message);
        }

        public override void Update()
        {
            if (!File.Exists(Filename))
                return;

            using (var db = TrustchainDatabase.Open(Filename))
            {
                db.KeyValue.Put("state", JsonConvert.SerializeObject(this));
            }

        }

    }
}
