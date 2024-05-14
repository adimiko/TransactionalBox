using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal sealed class CleanUpIdempotencyKeys : BackgroundProcessBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ISystemClock _systemClock;

        private readonly ICleanUpIdempotencyKeysSettings _settings;

        public CleanUpIdempotencyKeys(
            IServiceScopeFactory serviceScopeFactory,
            ISystemClock systemClock,
            ICleanUpIdempotencyKeysSettings settings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _systemClock = systemClock;
            _settings = settings;
        }

        protected override async Task Process(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var storage = scope.ServiceProvider.GetRequiredService<IInboxWorkerStorage>();

                    var numberOfRemovedKeys = await storage.RemoveExpiredIdempotencyKeys(_settings.BatchSize, _systemClock.UtcNow).ConfigureAwait(false);

                    while (numberOfRemovedKeys >= _settings.BatchSize)
                    {
                        numberOfRemovedKeys = await storage.RemoveExpiredIdempotencyKeys(_settings.BatchSize, _systemClock.UtcNow).ConfigureAwait(false);
                    }
                }

                await Task.Delay(_settings.Period, _systemClock.TimeProvider, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
