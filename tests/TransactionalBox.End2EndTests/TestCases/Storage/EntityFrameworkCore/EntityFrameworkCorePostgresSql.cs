using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore.DbContexts;

namespace TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore
{
    internal class EntityFrameworkCorePostgresSql
    {
        private readonly Func<Task<Dependencies>> _init;

        public EntityFrameworkCorePostgresSql()
        {
            _init = async () =>
            {
                var outboxDbContainer = new PostgreSqlBuilder()
                  .WithImage("postgres:15.1")
                  .Build();

                var inboxDbContainer = new PostgreSqlBuilder()
                  .WithImage("postgres:15.1")
                  .Build();

                await Task.WhenAll(outboxDbContainer.StartAsync(), inboxDbContainer.StartAsync());

                var outboxDependencies = new ServiceCollection();

                outboxDependencies.AddDbContextPool<OutboxDbContext>(x => x.UseNpgsql(outboxDbContainer.GetConnectionString()));

                outboxDependencies.AddLogging();
                outboxDependencies.AddTransactionalBox(
                    builder => builder.AddOutbox(x => x.UseEntityFramework<OutboxDbContext>()),
                    settings => settings.ServiceId = "OUTBOX");

                var outbox = outboxDependencies.BuildServiceProvider();

                var inboxDependencies = new ServiceCollection();

                inboxDependencies.AddDbContextPool<InboxDbContext>(x => x.UseNpgsql(inboxDbContainer.GetConnectionString()));

                inboxDependencies.AddLogging();
                inboxDependencies.AddTransactionalBox(
                    builder => builder.AddInbox(x => x.UseEntityFramework<InboxDbContext>()),
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
        }

        public End2EndTestCase GetEnd2EndTestCase()
        {
            return new End2EndTestCase(_init, nameof(EntityFrameworkCorePostgresSql));
        }
    }
}
