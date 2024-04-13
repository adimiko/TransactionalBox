using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TransactionalBox.DistributedLock.EntityFramework.Tests.SeedWork;
using TransactionalBox.KeyedInMemoryLock;
using Xunit;

namespace TransactionalBox.DistributedLock.EntityFramework.Tests
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

            services.AddDistributedLock<TestLock>(x => x.UseEntityFramework<TestDbContext>());

            // KeyedInMemoryLock is disabled because we want multiple tasks to query the database
            services.AddSingleton<IKeyedInMemoryLock, DisabledKeyedInMemoryLock>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();

            scope.ServiceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();
        }

        [Fact(DisplayName = "Distributed lock works correct in multi-thread enviroment")]
        public async Task Test1()
        {
            var timeProvider = TimeProvider.System;
            var lockTimeout = TimeSpan.FromSeconds(1);
            var checkingInterval = TimeSpan.FromMicroseconds(100);

            var distributedLock = _serviceProvider.GetRequiredService<IDistributedLock<TestLock>>();

            for (var i = 0; i < 3; i++)
            {
                // Different lock keys
                var lockKeyA = "A";
                var lockKeyB = "B";
                var lockKeyC = "C";

                var taskA1 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var taskB1 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var taskC1 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                var taskA2 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var taskB2 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var taskC2 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                var taskA3 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var taskB3 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var taskC3 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                await Task.WhenAll(taskA1, taskB1);

                Assert.True(taskA1.IsCompleted);
                Assert.True(taskB1.IsCompleted);
                Assert.True(taskC1.IsCompleted);

                // Second tasks should wait when locks will be released
                await Task.Delay(100);

                Assert.False(taskA2.IsCompleted, "Task A2 did not wait for task A1 to release the distributed lock");
                Assert.False(taskB2.IsCompleted, "Task B2 did not wait for task B1 to release the distributed lock");
                Assert.False(taskC2.IsCompleted, "Task C2 did not wait for task C1 to release the distributed lock");

                Assert.False(taskA3.IsCompleted, "Task A3 did not wait for task A1 to release the distributed lock");
                Assert.False(taskB3.IsCompleted, "Task B3 did not wait for task B1 to release the distributed lock");
                Assert.False(taskC3.IsCompleted, "Task C3 did not wait for task C1 to release the distributed lock");

                // When first tasks release locks, second tasks can continue
                await taskA1.Result.DisposeAsync();
                await taskB1.Result.DisposeAsync();
                await taskC1.Result.DisposeAsync();

                await Task.WhenAll(taskA2, taskB2, taskC2);

                Assert.True(taskA2.IsCompleted);
                Assert.True(taskB2.IsCompleted);
                Assert.True(taskC2.IsCompleted);

                Assert.False(taskA3.IsCompleted, "Task A3 did not wait for task A2 to release the distributed lock");
                Assert.False(taskB3.IsCompleted, "Task B3 did not wait for task B2 to release the distributed lock");
                Assert.False(taskC3.IsCompleted, "Task C3 did not wait for task C2 to release the distributed lock");

                await taskA2.Result.DisposeAsync();
                await taskB2.Result.DisposeAsync();
                await taskC2.Result.DisposeAsync();


                await Task.WhenAll(taskA3, taskB3, taskC3);

                Assert.True(taskA3.IsCompleted);
                Assert.True(taskB3.IsCompleted);
                Assert.True(taskC3.IsCompleted);

                await taskA3.Result.DisposeAsync();
                await taskB3.Result.DisposeAsync();
                await taskC3.Result.DisposeAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
