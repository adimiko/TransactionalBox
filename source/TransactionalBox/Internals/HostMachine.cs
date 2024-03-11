using System.Net;

namespace TransactionalBox.Internals
{
    internal sealed class HostMachine : IHostMachine
    {
        public string Name => Dns.GetHostName();
    }
}
