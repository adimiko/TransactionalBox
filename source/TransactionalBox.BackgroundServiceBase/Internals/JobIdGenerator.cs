using TransactionalBox.Internals;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    //TODO sequence based on timestamp + servicename + sequence
    internal sealed class JobIdGenerator
    {
        public string GetId(string machineName, string jobName, int instanceId)
        {
            return machineName + '-' + jobName + instanceId + Guid.NewGuid();
        }
    }
}
