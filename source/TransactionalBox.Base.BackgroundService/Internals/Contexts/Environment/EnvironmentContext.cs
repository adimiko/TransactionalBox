using System.Net;

namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.Environment
{
    internal sealed class EnvironmentContext : IEnvironmentContext
    {
        public string MachineName => !string.IsNullOrWhiteSpace(System.Environment.MachineName) ? System.Environment.MachineName : Dns.GetHostName();
    }
}
