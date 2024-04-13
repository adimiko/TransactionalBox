using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TransactionalBox.KeyedInMemoryLock;
using Xunit;

namespace TransactionalBox.DistributedLock.EntityFramework.Tests
{
    public sealed class Test : IAsyncLifetime
    {
        private IServiceProvider _serviceProvider;

        public async Task InitializeAsync()
        {
            var postgreSqlContainer = new PostgreSqlBuilder().Build();

            await postgreSqlContainer.StartAsync();

            var connectionString = postgreSqlContainer.GetConnectionString();

            IServiceCollection services = new ServiceCollection();

            services.AddDbContextPool<TestDbContext>(x => x.UseNpgsql(connectionString));

            services.AddDistributedLock<TestLock>(x => x.UseEntityFramework<TestDbContext>());

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();

            scope.ServiceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();
        }

        [Fact]
        public async Task Test1()
        {
            var timeProvider = TimeProvider.System;
            var lockTimeout = TimeSpan.FromMicroseconds(1000);
            var checkingInterval = TimeSpan.FromMicroseconds(10);

            var distributedLock = _serviceProvider.GetRequiredService<IDistributedLock<TestLock>>();

            for (var i = 0; i < 2; i++)
            {
                // Different lock keys
                var lockKeyA = "A";
                var lockKeyB = "B";

                //var distributedLock = _serviceProvider.GetRequiredService<IDistributedLock<TestLock>>();

                var taskA1 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var taskB1 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);

                var taskA2 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var taskB2 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);

                await Task.WhenAll(taskA1, taskB1);

                Assert.True(taskA1.IsCompleted);
                Assert.True(taskB1.IsCompleted);

                // Second tasks should wait when locks will be released
                Assert.False(taskA2.IsCompleted);
                Assert.False(taskB2.IsCompleted);

                // When first tasks release locks, second tasks can continue
                await taskA1.Result.DisposeAsync();
                await taskB1.Result.DisposeAsync();

                await Task.WhenAll(taskA2, taskB2);

                Assert.True(taskA2.IsCompleted);
                Assert.True(taskB2.IsCompleted);

                await taskA2.Result.DisposeAsync();
                await taskB2.Result.DisposeAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
