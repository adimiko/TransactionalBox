using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContextConstructor
    {
        JobId JobId { set; }

        string JobExecutorId { set; }

        JobName JobName { set; }
    }
}
