using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.Base.BackgroundService.Internals.JobExecutors
{
    internal sealed class JobRunner
    {
        private readonly IServiceProvider _serviceProvider;

        public JobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Run(Type jobType, CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                //TODO run maybe some class
                //TODO logic with execute logging etc. 
                // no throw exception
                //TODO JobExecutor
            });
        }
    }
}
