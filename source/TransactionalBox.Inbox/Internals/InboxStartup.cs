using Microsoft.Extensions.Hosting;
using TransactionalBox.Inbox.Internals.Transport.Topics;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxStartup : BackgroundService
    {
        private readonly ITopicsProvider _topicProvider;

        private readonly ITransportTopicsCreator _transportTopicsCreator;

        public InboxStartup(
            ITopicsProvider topicProvider,
            ITransportTopicsCreator transportTopicsCreator) 
        {
            _topicProvider = topicProvider;
            _transportTopicsCreator = transportTopicsCreator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topics = _topicProvider.Topics;

            await _transportTopicsCreator.Create(topics).ConfigureAwait(false);

            // startup sequence

            //TODO run BackgroundProcessed
            //TODO run EventHooks
        }
    }
}
