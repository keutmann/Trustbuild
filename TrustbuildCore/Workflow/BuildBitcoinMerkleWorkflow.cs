﻿using NBitcoin;
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

            using (var db = TrustchainDatabase.Open(Package.FilePath))
            {
                var leafNodes = new List<MerkleNodeModel>();
                var trusts = db.Trust.Select();
                foreach (var item in trusts)
                    leafNodes.Add(new MerkleNodeModel(item.TrustId, item));

                var merkleTree = new MerkleTree(leafNodes);
                var rootNode = merkleTree.Build();
                Package.RootHash = rootNode.Hash;

                Package.TimestampName = BlockchainName();

                foreach (var node in leafNodes)
                {
                    var trust = (TrustModel)node.Tag;
                    if (trust.Timestamp == null)
                        trust.Timestamp = new TimestampCollection(); 

                    trust.Timestamp[Package.TimestampName] = new TimestampModel
                    {
                        HashAlgorithm = "sha160",
                        Path = node.Path
                    };

                    db.Trust.Replace(trust);
                }
            }

            Context.Log("Merkle build of trust done");

            //Context.Enqueue(typeof(TimeStampAddWorkflow)); // Disabled for testing purposes
            Context.Enqueue(typeof(VacuumPackageWorkflow));
        }
    }
}
