using TransactionalBox.Base.Transport.InMemory.Internals;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Transport.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            configurator.UseInternalInMemory();
        }
    }
}
