using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    internal sealed class JobExecutionContext : IJobExecutionContext, IJobExecutionContextConstructor
    {
        public JobId JobId { get; set; }

        public string JobExecutorId { get; set; }

        public JobName JobName { get; set; }
    }
}
