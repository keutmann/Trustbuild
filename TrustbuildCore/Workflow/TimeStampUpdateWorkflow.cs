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
                foreach (var trust in trusts)
                {
                    if (trust.Timestamp == null)
                        continue;

                    if (!trust.Timestamp.ContainsKey(name))
                        continue;

                    trust.Timestamp[name].Path = Package.RootPath.Combine(trust.Timestamp[name].Path);

                    db.Trust.Replace(trust);
                }
            }

            Context.Log("Timestamp of trust done");
            Context.Enqueue(typeof(VacuumPackageWorkflow));
        }
    }
}
