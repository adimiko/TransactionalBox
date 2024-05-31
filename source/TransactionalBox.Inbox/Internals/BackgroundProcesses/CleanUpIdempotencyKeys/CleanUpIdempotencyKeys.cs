using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys.Logger;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal sealed class CleanUpIdempotencyKeys : BackgroundProcessBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ISystemClock _systemClock;

        private readonly ICleanUpIdempotencyKeysSettings _settings;

        private readonly ICleanUpIdempotencyKeysLogger _logger;

        public CleanUpIdempotencyKeys(
            IServiceScopeFactory serviceScopeFactory,
            ISystemClock systemClock,
            ICleanUpIdempotencyKeysSettings settings,
            ICleanUpIdempotencyKeysLogger logger)
            :base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _systemClock = systemClock;
            _settings = settings;
            _logger = logger;
        }

        protected override async Task Process(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var storage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                    var id = Guid.NewGuid();
                    var name = typeof(CleanUpIdempotencyKeys).Name;
                    long iteration = 1;
                    int numberOfRemovedKeys = 0;

                    do
                    {
                        numberOfRemovedKeys = await storage.RemoveExpiredIdempotencyKeys(_settings.MaxBatchSize, _systemClock.UtcNow).ConfigureAwait(false);

                        _logger.CleanedUp(name, id, iteration, numberOfRemovedKeys);

                        iteration++;
                    }
                    while (numberOfRemovedKeys >= _settings.MaxBatchSize);
                }

                await Task.Delay(_settings.Period, _systemClock.TimeProvider, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
