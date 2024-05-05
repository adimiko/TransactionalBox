using TransactionalBox.Base.BackgroundService.Internals.Launchers;

namespace TransactionalBox.Base.BackgroundService.Internals.Throttling
{
    internal interface IEvenDistributionOfJobsFactory
    {
        IEnumerable<Type> Create(IEnumerable<JobLaunchSettings> jobLaunchSettings);
    }
}
