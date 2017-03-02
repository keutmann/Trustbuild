using NBitcoin;
using System;
using System.Net;
using System.Threading.Tasks;
using TrustbuildCore.Service;
using TrustchainCore.Data;
using TrustchainCore.Extensions;
using TrustchainCore.Service;

namespace TrustbuildCore.Workflow
{
    public class TimeStampAddWorkflow : WorkflowPackage
    {

        public override void Execute()
        {
            var stampService = new TruststampService();
            var result = stampService.AddProof(Package.RootHash);

            if (!result["hash"].HasValues)
                throw new ApplicationException("Error missing hash from AddProof");

            Context.Log("Timestamp of trust submitted");
            Context.Enqueue(typeof(TimeStampWaitWorkflow));
        }
    }
}
