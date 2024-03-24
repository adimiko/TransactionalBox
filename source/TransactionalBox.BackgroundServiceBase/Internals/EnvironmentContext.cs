using System.Net;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal sealed class EnvironmentContext : IEnvironmentContext
    {
        public string MachineName => !string.IsNullOrWhiteSpace(Environment.MachineName) ? Environment.MachineName : Dns.GetHostName();
    }
}
