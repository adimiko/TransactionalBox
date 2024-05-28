using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Reflection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore.DbContexts;
using Xunit.Abstractions;

namespace TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore
{
    internal sealed class EntityFrameworkCoreSqlServer
    {
        private Assembly Assembly => typeof(EntityFrameworkCoreSqlServer).Assembly;

        private readonly Func<ITestOutputHelper, Task<Dependencies>> _init;

        private readonly Func<Task> _cleanUp;

        private MsSqlContainer _outboxContainer;

        private MsSqlContainer _inboxContainer;

        public EntityFrameworkCoreSqlServer()
        {
            _init = async (ITestOutputHelper output) =>
            {
                _outboxContainer = new MsSqlBuilder().Build();

                _inboxContainer = new MsSqlBuilder().Build();

                await Task.WhenAll(_outboxContainer.StartAsync(), _inboxContainer.StartAsync());

                var outboxDependencies = new ServiceCollection();

                outboxDependencies.AddDbContextPool<OutboxDbContext>(x => x.UseSqlServer(_outboxContainer.GetConnectionString()));

                outboxDependencies.AddLogging((builder) => builder.AddXUnit(output));
                outboxDependencies.AddTransactionalBox(
                    builder => builder.AddOutbox(x => x.UseEntityFramework<OutboxDbContext>(), assemblyConfiguraton: a => a.RegisterFromAssemblies(Assembly)),
                    settings => settings.ServiceId = "OUTBOX");

                var outbox = outboxDependencies.BuildServiceProvider();

                var inboxDependencies = new ServiceCollection();

                inboxDependencies.AddDbContextPool<InboxDbContext>(x => x.UseSqlServer(_inboxContainer.GetConnectionString()));

                inboxDependencies.AddLogging((builder) => builder.AddXUnit(output));
                inboxDependencies.AddTransactionalBox(
                    builder => builder.AddInbox(x => x.UseEntityFramework<InboxDbContext>(), assemblyConfiguraton: a => a.RegisterFromAssemblies(Assembly)),
                    settins => settins.ServiceId = "INBOX");

                inboxDependencies.AddSingleton<InboxVerifier>();

                var inbox = inboxDependencies.BuildServiceProvider();

                using (var scope = outbox.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<OutboxDbContext>().Database.EnsureCreatedAsync();
                }

                using (var scope = inbox.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<InboxDbContext>().Database.EnsureCreatedAsync();
                }

                return new Dependencies(outbox, inbox);
            };

            _cleanUp = async () =>
            {
                await Task.WhenAll(_outboxContainer.StopAsync(), _inboxContainer.StopAsync());
            };
        }

        public End2EndTestCase GetEnd2EndTestCase()
        {
            return new End2EndTestCase(_init, _cleanUp, nameof(EntityFrameworkCoreSqlServer));
        }
    }
}
