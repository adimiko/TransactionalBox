using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContextConstructor
    {
        JobId JobId { set; }

        JobExecutorId JobExecutorId { set; }

        JobName JobName { set; }
    }
}
