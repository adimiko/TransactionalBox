using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;

namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution
{
    public interface IJobExecutionContext
    {
        JobId JobId { get; }

        JobExecutorId JobExecutorId { get; }

        JobName JobName { get; }

        ProcessingState ProcessingState { get; }
    }
}
