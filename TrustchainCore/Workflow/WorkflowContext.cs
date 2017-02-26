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
        public Queue<WorkflowBase> Workflows = new Queue<WorkflowBase>();

        //public Dictionary<string, object> KeyValueTable = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public virtual WorkflowState State { get; set; }

        public virtual async Task Execute()
        {
            var first = Workflows.Dequeue();
            if (first == null)
                return;

            // Make sure that first workflow Initialize are run syncronized!
            if (!first.Initialize())
                return;

            await Task.Run(() => {
                try
                {
                    // Now run rest as async!
                    first.Execute();

                    while (Workflows.Count > 0) 
                    {
                        using (var wf = Workflows.Dequeue())
                        {
                            if (wf.Initialize()) // Initialize and make sure that dependencies are ready
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

        public virtual void Enqueue(WorkflowBase wf)
        {
            wf.Context = this;
            Workflows.Enqueue(wf);
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
