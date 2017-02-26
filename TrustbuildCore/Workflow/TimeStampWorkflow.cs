using NBitcoin;
using TrustbuildCore.Service;
using TrustchainCore.Data;
using TrustchainCore.Extensions;

namespace TrustbuildCore.Workflow
{
    public class TimeStampWorkflow : WorkflowPackage
    {
        public override void Execute()
       {
            Context.Log("Timestamp of trust started");
            Context.Update();

            var blockchainName = ("BTC" + ((App.BitcoinNetwork.Name.Equals(Network.Main.Name)) ? "" : "-testnet")).ToLower();
            var hash = Package.KeyValue[blockchainName + "root"];


            using (var db = TrustchainDatabase.Open(Package.Filename))
            {

                var trusts = db.Trust.Select();
                foreach (var item in trusts)
                {
                    if (item.Timestamp == null)
                        return;

                    // Calc merkle 
                }
            }

            Context.Log("Timestamp of trust done");
            Context.Enqueue(new FinalizePackageWorkflow());
            Context.Update();
        }
    }
}
