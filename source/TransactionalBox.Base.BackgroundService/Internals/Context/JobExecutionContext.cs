using TransactionalBox.Base.BackgroundService.Internals.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.Context
{
    internal sealed class JobExecutionContext : IJobExecutionContext, IJobExecutionContextConstructor
    {
        public JobId JobId { get; set; }

        public JobExecutorId JobExecutorId { get; set; }

        public JobName JobName { get; set; }

        public ProcessingState ProcessingState { get; set; }
    }
}
