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
        //public Queue<WorkflowBase> Workflows = new Queue<WorkflowBase>();

        //public Dictionary<string, object> KeyValueTable = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public virtual WorkflowState State { get; set; }

        public virtual async Task Execute()
        {
            var wffirst = GetNextWorkflow();
            // Make sure that first workflow Initialize are run syncronized!
            if (wffirst == null || !wffirst.Initialize())
                return;

            await Task.Run(() => {
                try
                {
                    // Now run rest as async!
                    wffirst.Execute();

                    while (State.WorkflowQueue.Count > 0) 
                    {
                        using (var wf = GetNextWorkflow())
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

        public virtual WorkflowBase GetNextWorkflow()
        {
            if (State.WorkflowQueue.Count == 0)
                return null;
            var wftype = State.WorkflowQueue.Dequeue();
            var instance = CreateInstance<WorkflowBase>(wftype);
            instance.Context = this;
            return instance;
        }



        public virtual T CreateInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public virtual void Enqueue(Type wftype)
        {
            State.WorkflowQueue.Enqueue(wftype);
        }

        public virtual void Enqueue(WorkflowBase wf)
        {
            State.WorkflowQueue.Enqueue(wf.GetType());
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
