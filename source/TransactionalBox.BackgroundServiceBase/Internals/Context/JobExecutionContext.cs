namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    internal sealed class JobExecutionContext : IJobExecutionContext, IJobExecutorIdExecutionContextConstructor, IJobIdExecutionContextConstructor
    {
        public string JobId { get; set; }

        public string JobExecutiorId { get; set; }
    }
}
