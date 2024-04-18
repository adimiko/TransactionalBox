using TransactionalBox.Base.BackgroundService.Internals.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.Context
{
    public interface IJobExecutionContextConstructor
    {
        JobId JobId { set; }

        JobExecutorId JobExecutorId { set; }

        JobName JobName { set; }

        ProcessingState ProcessingState { set; }
    }
}
