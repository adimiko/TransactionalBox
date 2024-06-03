﻿using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox.Logger;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base;
using TransactionalBox.Inbox.Internals.Decompression;
using TransactionalBox.Inbox.Internals.Hooks.Events;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Inbox.Internals.Transport.Topics;
using TransactionalBox.Internals;
using TransactionalBox.Inbox.Internals.Contexts;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox
{
    internal sealed class AddMessagesToInbox : BackgroundProcessBase
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDecompressionFactory _decompressionFactory;

        private readonly IInboxTransport _inboxWorkerTransport;

        private readonly ISystemClock _systemClock;

        private readonly ITopicsProvider _topicsProvider;

        private readonly IAddMessagesToInboxSettings _settings;

        private readonly IEventHookPublisher _eventHookPublisher;

        private readonly IAddMessagesToInboxLogger _logger;

        private readonly IInboxContext _inboxContext;

        public AddMessagesToInbox(
            IServiceProvider serviceProvider,
            IDecompressionFactory decompressionFactory,
            IInboxTransport inboxWorkerTransport,
            ISystemClock systemClock,
            ITopicsProvider topicsProvider,
            IAddMessagesToInboxSettings settings,
            IEventHookPublisher eventHookPublisher,
            IAddMessagesToInboxLogger logger,
            IInboxContext inboxContext)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
            _decompressionFactory = decompressionFactory;
            _inboxWorkerTransport = inboxWorkerTransport;
            _systemClock = systemClock;
            _topicsProvider = topicsProvider;
            _settings = settings;
            _eventHookPublisher = eventHookPublisher;
            _logger = logger;
            _inboxContext = inboxContext;
        }

        protected override async Task Process(CancellationToken stoppingToken)
        {
            //TODO log per pentla
            await foreach (var messagesFromTransport in _inboxWorkerTransport.GetMessages(_topicsProvider.Topics, stoppingToken).ConfigureAwait(false))
            {
                var service = _inboxContext.Id;

                var decompression = _decompressionFactory.GetDecompression(messagesFromTransport.Compression);

                var decompressedMessagesFromTransport = await decompression.Decompress(messagesFromTransport.Payload).ConfigureAwait(false);

                using (var scope = _serviceProvider.CreateScope())
                {
                    //const string separator = "⸘";
                    //TODO #27

                    var inboxMessages = JsonSerializer.Deserialize<IEnumerable<InboxMessageStorage>>(decompressedMessagesFromTransport);

                    foreach(var inboxMessage in inboxMessages)
                    {
                        inboxMessage.OccurredUtc = inboxMessage.OccurredUtc.ToUniversalTime();
                    }

                    var storage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                    var idempotentMessages = inboxMessages.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                    var result = await storage.AddRange(inboxMessages, idempotentMessages).ConfigureAwait(false);

                    if (result == AddRangeToInboxStorageResult.Success) 
                    {
                        await _eventHookPublisher.PublishAsync<AddedMessagesToInbox>().ConfigureAwait(false);

                        continue;
                    }

                    do
                    {
                        var duplicatedInboxKeys = new List<DuplicatedInboxKey>();

                        var existIdempotentInboxKeys = await storage.GetExistIdempotentInboxKeysBasedOn(inboxMessages).ConfigureAwait(false);

                        var existIds = existIdempotentInboxKeys.Select(x => x.Id);

                        var duplicatedIds = inboxMessages.Where(x => existIds.Contains(x.Id)).Select(x => x.Id);

                        //TODO log duplicatedIds as Warning

                        var inboxMessagesToSave = inboxMessages.Where(x => existIds.Contains(x.Id));

                        var idempotentMessagesToSave = inboxMessagesToSave.Select(x => new IdempotentInboxKey(x.Id, _settings.DefaultTimeToLiveIdempotencyKey, _systemClock.TimeProvider));

                        result = await storage.AddRange(inboxMessages, idempotentMessagesToSave);
                    }
                    while (result == AddRangeToInboxStorageResult.Failure);

                    await _eventHookPublisher.PublishAsync<AddedMessagesToInbox>().ConfigureAwait(false);
                }
            }
        }
    }
}
