using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal sealed class CleanUpIdempotencyKeys : BackgroundService
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO to refactor
            var periodicTimer = new PeriodicTimer(_settings.Period, _systemClock.TimeProvider);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    do
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
                    }
                    while (await periodicTimer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false));
                }
                catch (Exception)
                {
                    //TODO logging
                }
            }
        }
    }
}
