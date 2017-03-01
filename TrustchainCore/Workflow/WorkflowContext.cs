using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class WorkflowContext
    {
        public Queue<Type> WorkflowQueue { get; set; }
        public WorkflowStatus Status { get; set; }

        public List<WorkflowLog> Logs { get; set; }
        public Dictionary<string, object> KeyValue { get; set; }

        public WorkflowContext()
        {
            WorkflowQueue = new Queue<Type>();
            Logs = new List<WorkflowLog>();
            KeyValue = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

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

                    while (WorkflowQueue.Count > 0)
                    {
                        using (var wf = GetNextWorkflow())
                        {
                            if (wf.Initialize()) // Initialize and make sure that dependencies are ready
                            {
                                wf.Execute();
                                Update();
                            }
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
            if (WorkflowQueue.Count == 0)
                return null;
            var wftype = WorkflowQueue.Dequeue();
            
            if (!string.IsNullOrEmpty(KeyValue[wftype.Name] as string))
                return (WorkflowBase)JsonConvert.DeserializeObject(KeyValue[wftype.Name] as string);
            else
                return CreateInstance<WorkflowBase>(wftype);
        }



        public virtual T CreateInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public virtual void Enqueue(Type wftype)
        {
            WorkflowQueue.Enqueue(wftype);
        }

        public virtual void Update()
        {
        }

        public virtual void Log(string message)
        {
            Logs.Add(new WorkflowLog { Message = message });
        }


        static Random rand = new Random();
        public void RandomWork()
        {
            int i = rand.Next(1000000);
            Thread.SpinWait(i);
        }
    }
}
