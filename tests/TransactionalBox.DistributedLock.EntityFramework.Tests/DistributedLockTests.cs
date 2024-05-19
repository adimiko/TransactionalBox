using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Tests.SeedWork;
using TransactionalBox.Internals.DistributedLock.Extensions;
using TransactionalBox.Internals.KeyedInMemoryLock;
using Xunit;

namespace TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Tests
{
    public sealed class DistributedLockTests : IAsyncLifetime
    {
        private IServiceProvider _serviceProvider;

        public async Task InitializeAsync()
        {
            var postgreSqlContainer = new PostgreSqlBuilder().Build();

            await postgreSqlContainer.StartAsync();

            var connectionString = postgreSqlContainer.GetConnectionString();

            IServiceCollection services = new ServiceCollection();

            services.AddDbContextPool<TestDbContext>(x => x.UseNpgsql(connectionString));

            services.AddDistributedLock<TestLock>(x => x.UseEntityFrameworkCore<TestDbContext>());

            // KeyedInMemoryLock is disabled because we want multiple tasks to query the database
            services.AddSingleton<IKeyedInMemoryLock, DisabledKeyedInMemoryLock>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();

            scope.ServiceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();
        }

        [Fact(DisplayName = "Distributed lock works correct in multi-thread enviroment")]
        public async Task Test()
        {
            var timeProvider = TimeProvider.System;
            var lockTimeout = TimeSpan.FromSeconds(5);
            var checkingInterval = TimeSpan.FromMicroseconds(10);

            var distributedLock = _serviceProvider.GetRequiredService<IDistributedLock<TestLock>>();

            const string lockKeyA = "A";
            const string lockKeyB = "B";
            const string lockKeyC = "C";

            for (var i = 0; i < 2; i++)
            {
                var a1 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var b1 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var c1 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                await Task.WhenAll(a1, b1, c1);

                var a2 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var b2 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var c2 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                Assert.True(a1.IsCompleted, Message(nameof(a1)));
                Assert.True(b1.IsCompleted, Message(nameof(b1)));
                Assert.True(c1.IsCompleted, Message(nameof(c1)));

                await Task.Delay(250);

                // Second tasks should wait when locks will be released
                Assert.False(a2.IsCompleted, Message(nameof(a2), nameof(a1)));
                Assert.False(b2.IsCompleted, Message(nameof(b2), nameof(b1)));
                Assert.False(c2.IsCompleted, Message(nameof(c2), nameof(c1)));

                // When first tasks release locks, second tasks can continue
                await a1.Result.DisposeAsync();
                await b1.Result.DisposeAsync();
                await c1.Result.DisposeAsync();

                await Task.WhenAll(a2, b2, c2);

                Assert.True(a2.IsCompleted, Message(nameof(a2)));
                Assert.True(b2.IsCompleted, Message(nameof(b2)));
                Assert.True(c2.IsCompleted, Message(nameof(c2)));

                await a2.Result.DisposeAsync();
                await b2.Result.DisposeAsync();
                await c2.Result.DisposeAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private string Message(string x) => $"Task {x} should be completed.";

        private string Message(string x, string y) => $"Task {x} did not wait for task {y} to release the distributed lock.";
    }
}
