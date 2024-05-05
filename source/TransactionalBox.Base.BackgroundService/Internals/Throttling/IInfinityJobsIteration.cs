using TransactionalBox.Base.BackgroundService.Internals.Launchers;

namespace TransactionalBox.Base.BackgroundService.Internals.Throttling
{
    internal interface IInfinityJobsIteration
    {
        public IAsyncEnumerable<Type> GetJobType(IEnumerable<JobLaunchSettings> jobLaunchSettings, CancellationToken cancellationToken);
    }
}
