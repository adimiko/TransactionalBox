using TransactionalBox.Base.BackgroundService.Internals.Context.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.Context
{
    public interface IJobExecutionContext
    {
        JobId JobId { get; }

        JobExecutorId JobExecutorId { get; }

        JobName JobName { get; }

        ProcessingState ProcessingState { get; }
    }
}
