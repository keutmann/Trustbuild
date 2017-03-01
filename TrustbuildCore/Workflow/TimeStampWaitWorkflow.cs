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
    public class TimeStampWaitWorkflow : WorkflowPackage
    {

        public override void Execute()
        {
            var stampService = new TruststampService();
            var result = stampService.GetProof(Package.RootHash);

            if (!result["path"].HasValues)
            {
                if (Package.ProofWaitCount > 36)
                {
                    Package.Log("Timeout on Timestamp service.");
                    Package.Enqueue(typeof(FailueWorkflow));
                }
                else
                    SleepWorkflow.Enqueue(Context, DateTime.Now.AddMinutes(10), this.GetType());
                return;
            }

            Package.RootPath = result["path"].ToBytes();

            Context.Enqueue(typeof(TimeStampUpdateWorkflow));
        }
    }
}
