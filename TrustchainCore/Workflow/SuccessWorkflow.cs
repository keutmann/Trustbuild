namespace TrustchainCore.Workflow
{
    public class SuccessWorkflow : WorkflowBase
    {
        public override void Execute()
        {
            Context.SetStatus(WorkflowStatus.Finished);
            Context.Log("Workflow completed successfully");
            Context.Update();
        }
    }
}
