using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;

namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution
{
    internal sealed class JobExecutionContext : IJobExecutionContext, IJobExecutionContextConstructor
    {
        public JobId JobId { get; set; }

        public JobExecutorId JobExecutorId { get; set; }

        public JobName JobName { get; set; }

        public ProcessingState ProcessingState { get; set; }
    }
}
