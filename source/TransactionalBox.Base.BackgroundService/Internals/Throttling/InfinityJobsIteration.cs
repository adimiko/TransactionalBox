using TransactionalBox.Base.BackgroundService.Internals.Launchers;

namespace TransactionalBox.Base.BackgroundService.Internals.Throttling
{
    internal sealed class InfinityJobsIteration : IInfinityJobsIteration
    {
        private readonly IEvenDistributionOfJobsFactory _evenDistributionOfJobsFactory;

        public InfinityJobsIteration(IEvenDistributionOfJobsFactory evenDistributionOfJobsFactory) 
        {
            _evenDistributionOfJobsFactory = evenDistributionOfJobsFactory;
        }

        public async IAsyncEnumerable<Type> GetJobType(IEnumerable<JobLaunchSettings> jobLaunchSettings, CancellationToken cancellationToken)
        {
            var jobTypes = _evenDistributionOfJobsFactory.Create(jobLaunchSettings);

            while (!cancellationToken.IsCancellationRequested) 
            {
                foreach (var jobType in jobTypes)
                {
                    yield return jobType;
                }
            }

            await Task.CompletedTask;
        }
    }
}
