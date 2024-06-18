using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Testcontainers.PostgreSql;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore.DbContexts;
using Xunit.Abstractions;

namespace TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore
{
    internal class EntityFrameworkCorePostgresSql
    {
        private Assembly Assembly => typeof(EntityFrameworkCorePostgresSql).Assembly;

        private readonly Func<ITestOutputHelper, Task<Dependencies>> _init;

        private readonly Func<Task> _cleanUp;

        private PostgreSqlContainer _outboxContainer;

        private PostgreSqlContainer _inboxContainer;

        private IServiceProvider _outbox;

        private IServiceProvider _inbox;

        public EntityFrameworkCorePostgresSql()
        {
            _init = async (ITestOutputHelper output) =>
            {
                _outboxContainer = new PostgreSqlBuilder()
                  .WithImage("postgres:15.1")
                  .Build();

                _inboxContainer = new PostgreSqlBuilder()
                  .WithImage("postgres:15.1")
                  .Build();

                await Task.WhenAll(_outboxContainer.StartAsync(), _inboxContainer.StartAsync());

                var outboxDependencies = new ServiceCollection();

                outboxDependencies.AddDbContextPool<OutboxDbContext>(x => x.UseNpgsql(_outboxContainer.GetConnectionString()));

                outboxDependencies.AddLogging((builder) => builder.AddXUnit(output));
                outboxDependencies.AddTransactionalBox(
                    builder => builder.AddOutbox(x => x.UseEntityFrameworkCore<OutboxDbContext>(), assemblyConfiguraton: a => a.RegisterFromAssemblies(Assembly)),
                    settings => settings.ServiceId = "OUTBOX");

                _outbox = outboxDependencies.BuildServiceProvider();

                var inboxDependencies = new ServiceCollection();

                inboxDependencies.AddDbContextPool<InboxDbContext>(x => x.UseNpgsql(_inboxContainer.GetConnectionString()));

                inboxDependencies.AddLogging((builder) => builder.AddXUnit(output));
                inboxDependencies.AddTransactionalBox(
                    builder => builder.AddInbox(x => x.UseEntityFrameworkCore<InboxDbContext>(), assemblyConfiguraton: a => a.RegisterFromAssemblies(Assembly)),
                    settins => settins.ServiceId = "INBOX");

                inboxDependencies.AddSingleton<InboxVerifier>();

                _inbox = inboxDependencies.BuildServiceProvider();

                using (var scope = _outbox.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<OutboxDbContext>().Database.EnsureCreatedAsync();
                }

                using (var scope = _inbox.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<InboxDbContext>().Database.EnsureCreatedAsync();
                }

                var outboxHostedServices = _outbox.GetServices<IHostedService>();

                foreach (var outboxHostedService in outboxHostedServices)
                {
                    await outboxHostedService.StartAsync(CancellationToken.None).ConfigureAwait(false);
                }

                var inboxHostedServices = _inbox.GetServices<IHostedService>();

                foreach (var inboxHostedService in inboxHostedServices)
                {
                    await inboxHostedService.StartAsync(CancellationToken.None).ConfigureAwait(false);
                }

                return new Dependencies(_outbox, _inbox);
            };

            _cleanUp = async () =>
            {
                var outboxHostedServices = _outbox.GetServices<IHostedService>();

                foreach (var outboxHostedService in outboxHostedServices)
                {
                    await outboxHostedService.StopAsync(CancellationToken.None).ConfigureAwait(false); ;
                }

                var inboxHostedServices = _inbox.GetServices<IHostedService>();

                foreach (var inboxHostedService in inboxHostedServices)
                {
                    await inboxHostedService.StopAsync(CancellationToken.None).ConfigureAwait(false); ;
                }

                await Task.WhenAll(_outboxContainer.StopAsync(), _inboxContainer.StopAsync());
            };
        }

        public End2EndTestCase GetEnd2EndTestCase()
        {
            return new End2EndTestCase(_init, _cleanUp, nameof(EntityFrameworkCorePostgresSql));
        }
    }
}
