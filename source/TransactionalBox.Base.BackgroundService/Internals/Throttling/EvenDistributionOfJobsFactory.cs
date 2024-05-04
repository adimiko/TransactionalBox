using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalBox.Base.BackgroundService.Internals.Launchers;

namespace TransactionalBox.Base.BackgroundService.Internals.Throttling
{
    internal sealed class EvenDistributionOfJobsFactory
    {
        //TODO refactor algorithm
        internal IEnumerable<Type> Create(IEnumerable<JobLaunchSettings> jobLaunchSettings)
        {
            double expectedSize = jobLaunchSettings.Sum(x => x.NumberOfInstances);
            
            var ratios = new Dictionary<Type, double>();

            var listOfTempsVars = new Dictionary<Type, double>();

            foreach (var job in jobLaunchSettings) 
            {
                var ratio = Math.Round((double)job.NumberOfInstances / expectedSize, 2);

                ratios.Add(job.JobType, ratio);

                listOfTempsVars.Add(job.JobType, 0);
            }

            var evenDistributionOfJobs = new List<Type>();


            while (evenDistributionOfJobs.Count < expectedSize)
            {
                foreach (var ratio in ratios) 
                {
                    var key = ratio.Key;
                    var diff = ratio.Value;

                    var tempValue = listOfTempsVars.GetValueOrDefault(key);

                    tempValue += diff;

                    if (tempValue > 1 && evenDistributionOfJobs.Count < expectedSize)
                    {
                        evenDistributionOfJobs.Add(key);
                        tempValue -= 1;
                    }

                    listOfTempsVars[key] = tempValue;
                }
            }

            return evenDistributionOfJobs;
        }

    }

    // then use SemaphoreSlim with MaxDegreeParallelism from EvenDistributionOfJobs
    // jobType has own state
}
