using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustbuildCore.Business;
using TrustbuildCore.Service;
using TrustchainCore.Business;
using TrustchainCore.Data;
using TrustchainCore.Model;
using TrustchainCore.Extensions;

namespace TrustbuildCore.Workflow
{
    public class BuildBitcoinMerkleWorkflow : WorkflowPackage
    {
        public static string BlockchainName()
        {
            return ("BTC" + ((App.BitcoinNetwork.Name.Equals(Network.Main.Name)) ? "" : "-testnet")).ToLower();
        }

        public override void Execute()
        {
            Context.Log("Merkle build of trust started");
            Context.Update();

            using (var db = TrustchainDatabase.Open(Package.Filename))
            {
                var leafNodes = new List<MerkleNodeModel>();
                var trusts = db.Trust.Select();
                foreach (var item in trusts)
                    leafNodes.Add(new MerkleNodeModel(item.TrustId, item));

                var merkleTree = new MerkleTree(leafNodes);
                var rootNode = merkleTree.Build();

                var name = BlockchainName();
                Package.KeyValue[name + "root"] = rootNode.Hash.ConvertToHex();

                foreach (var node in leafNodes)
                {
                    var trust = (TrustModel)node.Tag;
                    trust.Timestamp[name] = new TimestampModel
                    {
                        HashAlgorithm = "sha160",
                        Path = node.Path
                    };

                }
            }

            Context.Log("Merkle build of trust done");
            Context.Enqueue(new TimeStampWorkflow());
            Context.Update();
        }
    }
}
