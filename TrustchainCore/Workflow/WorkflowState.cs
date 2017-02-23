using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Workflow
{
    public enum WorkflowStatus : int
    {
        Ready,
        Running,
        Finished,
        Failed
    }

    public class WorkflowState
    {
        public string ExecutingWorkflowName { get; set; }
        public WorkflowStatus Status { get; set; }

        public List<WorkflowLog> Log { get; set; }

        public WorkflowState()
        {
            Log = new List<WorkflowLog>();
        }
    }
}
