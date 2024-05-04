using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.Base.BackgroundService.Internals.Throttling
{
    internal class EvenDistributionOfJobs
    {
        // TODO expected array size
        // set ratio for all tasks
        // temp variable for all job types (add ratio)
        // when temp var is grater than 1 add to eventDistributionArray and -1 for temp varaible
        // repeate when table will have expected size
        // Write unit tests for this algoritm
    }

    // then use SemaphoreSlim with MaxDegreeParallelism from EvenDistributionOfJobs
    // jobType has own state
}
