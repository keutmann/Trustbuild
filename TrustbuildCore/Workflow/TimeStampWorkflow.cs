﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Data;
using TrustchainCore.Workflow;

namespace TrustbuildCore.Workflow
{
    public class TimeStampWorkflow : WorkflowPackage
    {
        public override void Execute()
       {
            Context.Log("Timestamp of trust started");
            Context.Update();

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
            Context.Push(new FinalizePackageWorkflow());
            Context.Update();
        }
    }
}
