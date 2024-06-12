using Microsoft.Extensions.Hosting;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.Base;
using TransactionalBox.Internals.Inbox.Hooks.Events;
using TransactionalBox.Internals.Inbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Transport.Topics;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.Inbox
{
    internal sealed class InboxStartup : BackgroundService
    {
        private readonly ITopicsProvider _topicProvider;

        private readonly ITransportTopicsCreator _transportTopicsCreator;

        private readonly IEnumerable<BackgroundProcessBase> _backgroundProcesses;

        private readonly IEventHookPublisher _eventHookPublisher;

        public InboxStartup(
            ITopicsProvider topicProvider,
            ITransportTopicsCreator transportTopicsCreator,
            IEnumerable<BackgroundProcessBase> backgroundProcesses,
            IEventHookPublisher eventHookPublisher)
        {
            _topicProvider = topicProvider;
            _transportTopicsCreator = transportTopicsCreator;
            _backgroundProcesses = backgroundProcesses;
            _eventHookPublisher = eventHookPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topics = _topicProvider.Topics;

            await _transportTopicsCreator.Create(topics).ConfigureAwait(false);

            // startup sequence
            await _eventHookPublisher.PublishAsync<AddedMessagesToInbox>().ConfigureAwait(false);

            foreach (var backgroundProcess in _backgroundProcesses)
            {
                _ = backgroundProcess.ExecuteAsync(stoppingToken);
            }
        }
    }
}
