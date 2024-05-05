using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TransactionalBox.Base.BackgroundService.Internals.Throttling;
using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Base.BackgroundService.Tests.Internals.Throttling.TestsJobs;

namespace TransactionalBox.Base.BackgroundService.Tests.Internals.Throttling
{
    public class EvenDistributionOfJobsFactoryTests
    {
        private EvenDistributionOfJobsFactory factory = new EvenDistributionOfJobsFactory();

        [Fact]
        public void Test()
        {
            // Arrange
            var list = new List<JobLaunchSettings>
            {
                new JobLaunchSettings(typeof(Job1), 1),
                new JobLaunchSettings(typeof(Job2), 1),
                new JobLaunchSettings(typeof(Job3), 8),
                new JobLaunchSettings(typeof(Job4), 1),
                new JobLaunchSettings(typeof(Job5), 1),
            };

            // Act
            var x = factory.Create(list);

            // Assert
            Assert.Equal(12, x.Count());
        }
    }
}
