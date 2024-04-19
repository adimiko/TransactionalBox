using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;

namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution
{
    public interface IJobExecutionContextConstructor
    {
        JobId JobId { set; }

        JobExecutorId JobExecutorId { set; }

        JobName JobName { set; }

        ProcessingState ProcessingState { set; }
    }
}
