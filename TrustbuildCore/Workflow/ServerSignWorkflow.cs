using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustbuildCore.Business;
using TrustchainCore.Data;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class ServerSignWorkflow : WorkflowPackage
    {
        public override void Execute()
       {
            Context.Log("Server sign of trust started");
            Context.Update();

            using (var db = TrustchainDatabase.Open(Package.Filename))
            {

                var trusts = db.Trust.SelectServerUnsigned();
                foreach (var item in trusts)
                {
                    var hash = new NBitcoin.uint256(item.TrustId);
                    item.Server.Signature = ServerIdentity.Current.PrivateKey.SignCompact(hash);
                    db.Trust.Replace(item);
                }
            }

            Context.Log("Server sign of trust done");
            Context.Enqueue(new BuildBitcoinMerkleWorkflow());
            Context.Update();
        }
    }
}
