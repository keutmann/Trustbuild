using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrustchainCore.Extensions;

namespace TrustchainCore.Workflow
{
    public class WorkflowContext
    {
        public Stack<WorkflowBase> Workflows = new Stack<WorkflowBase>();

        //public Dictionary<string, object> KeyValueTable = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public virtual WorkflowState State { get; set; }

        public virtual async Task Execute()
        {
            var first = Workflows.Pop();
            if (first == null)
                return;

            // Make sure that first workflow Initialize are run syncronized!
            if (!InitializeWorkflowForExecution(first))
                return;

            await Task.Run(() => {
                try
                {
                    // Now run rest as async!
                    first.Execute();

                    while (Workflows.Count > 0) 
                    {
                        using (var wf = Workflows.Pop())
                        {
                            if (InitializeWorkflowForExecution(wf)) // Initialize and make sure that dependencies are ready
                                wf.Execute();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            });

            
        }

        public virtual bool InitializeWorkflowForExecution(WorkflowBase wf)
        {
            State.ExecutingWorkflowName = wf.GetType().FullName;
            return wf.Initialize();
        }

        public virtual void Push(string fullname)
        {
            Workflows.Push(CreateInstance(fullname));
        }

        public virtual void Push(WorkflowBase wf)
        {
            wf.Context = this;
            Workflows.Push(wf);
        }

        public virtual WorkflowBase CreateInstance(string fullname)
        {
            var workflowType = Type.GetType(fullname, true);

            var wf = (WorkflowBase)Activator.CreateInstance(workflowType);
            wf.Context = this;
            return wf;
        }

        public virtual void SetStatus(WorkflowStatus status)
        {
            State.Status = status;
        }

        public virtual void Update()
        {
        }

        public virtual void Log(string message)
        {
            State.Log.Add(new WorkflowLog { Message = message });
        }


        static Random rand = new Random();
        public void RandomWork()
        {
            int i = rand.Next(1000000);
            Thread.SpinWait(i);
        }


    }
}
