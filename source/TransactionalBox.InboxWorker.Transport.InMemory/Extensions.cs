using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.Base.Transport.InMemory.Internals;

namespace TransactionalBox.InboxWorker.Transport.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxWorkerTransportConfigurator configurator)
        {
            configurator.UseInternalInMemory();
        }
    }
}
