using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox
{
    public static class ExtensionUseRabbitMQ
    {
        public static void UseRabbitMQ(
            this IInboxTransportConfigurator inboxWorkerTransportConfigurator)
        {
            var services = inboxWorkerTransportConfigurator.Services; 
        }
    }
}
