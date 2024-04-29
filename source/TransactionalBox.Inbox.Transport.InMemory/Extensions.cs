using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Base.Transport.InMemory.Internals;

namespace TransactionalBox.Inbox.Transport.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxTransportConfigurator configurator) => configurator.UseInternalInMemory();
    }
}
