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
        public Queue<WorkflowBase> WorkflowQueue { get; set; }
        public WorkflowStatus Status { get; set; }

        public List<WorkflowLog> Log { get; set; }
        public Dictionary<string, string> KeyValue { get; set; }

        public WorkflowState()
        {
            WorkflowQueue = new Queue<WorkflowBase>();
            Log = new List<WorkflowLog>();
            KeyValue = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
