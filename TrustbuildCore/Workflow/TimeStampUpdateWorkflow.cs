using NBitcoin;
using System;
using System.Net;
using System.Threading.Tasks;
using TrustbuildCore.Service;
using TrustchainCore.Data;
using TrustchainCore.Extensions;
using TrustchainCore.Service;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class TimeStampUpdateWorkflow : WorkflowPackage
    {

        public override void Execute()
        {
            var name = Package.TimestampName;

            using (var db = TrustchainDatabase.Open(Package.Filename))
            {

                var trusts = db.Trust.Select();
                foreach (var item in trusts)
                {
                    if (item.Timestamp == null)
                        continue;

                    if (!item.Timestamp.ContainsKey(name))
                        continue;

                    item.Timestamp[name].Path = Package.RootPath.Combine(item.Timestamp[name].Path);
                }
            }

            Context.Log("Timestamp of trust done");
            Context.Enqueue(typeof(VacuumPackageWorkflow));
        }
    }
}
