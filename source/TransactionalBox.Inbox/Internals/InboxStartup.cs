using Microsoft.Extensions.Hosting;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base;
using TransactionalBox.Inbox.Internals.Transport.ContractsToImplement;
using TransactionalBox.Inbox.Internals.Transport.Topics;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxStartup : BackgroundService
    {
        private readonly ITopicsProvider _topicProvider;

        private readonly ITransportTopicsCreator _transportTopicsCreator;

        private readonly IEnumerable<BackgroundProcessBase> _backgroundProcesses;

        public InboxStartup(
            ITopicsProvider topicProvider,
            ITransportTopicsCreator transportTopicsCreator,
            IEnumerable<BackgroundProcessBase> backgroundProcesses) 
        {
            _topicProvider = topicProvider;
            _transportTopicsCreator = transportTopicsCreator;
            _backgroundProcesses = backgroundProcesses;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topics = _topicProvider.Topics;

            await _transportTopicsCreator.Create(topics).ConfigureAwait(false);

            // startup sequence

            foreach(var backgroundProcess in _backgroundProcesses) 
            {
                _ = backgroundProcess.ExecuteAsync(stoppingToken);
            }

            //TODO run EventHooks
        }
    }
}
