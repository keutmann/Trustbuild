using NBitcoin;
using System.Net;
using System.Threading.Tasks;
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

            var name = BuildBitcoinMerkleWorkflow.BlockchainName();
            var roothash = Package.KeyValue[name + "root"];
            var url = "/" + roothash;
            var path = new byte[0];

            using (WebClient client = new WebClient())
            {
                var task = client.DownloadDataTaskAsync(url);
                task.Wait();
                path = task.Result;
            }

            using (var db = TrustchainDatabase.Open(Package.Filename))
            {

                var trusts = db.Trust.Select();
                foreach (var item in trusts)
                {
                    if (item.Timestamp == null)
                        continue;

                    if (!item.Timestamp.ContainsKey(name))
                        continue;

                    item.Timestamp[name].Path = path.Combine(item.Timestamp[name].Path);
                }
            }

            Context.Log("Timestamp of trust done");
            Context.Enqueue(typeof(FinalizePackageWorkflow));
            Context.Update();
        }
    }
}
