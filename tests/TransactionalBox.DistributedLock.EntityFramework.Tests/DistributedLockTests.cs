﻿using Microsoft.EntityFrameworkCore;
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

            for (var i = 0; i < 5; i++)
            {
                // Different lock keys
                var lockKeyA = "A";
                var lockKeyB = "B";
                var lockKeyC = "C";

                var a1 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var b1 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var c1 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                var a2 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var b2 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var c2 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                var a3 = distributedLock.Acquire(lockKeyA, timeProvider, lockTimeout, checkingInterval);
                var b3 = distributedLock.Acquire(lockKeyB, timeProvider, lockTimeout, checkingInterval);
                var c3 = distributedLock.Acquire(lockKeyC, timeProvider, lockTimeout, checkingInterval);

                await Task.WhenAll(a1, b1, c1);

                Assert.True(a1.IsCompleted, Message(nameof(a1)));
                Assert.True(b1.IsCompleted, Message(nameof(b1)));
                Assert.True(c1.IsCompleted, Message(nameof(c1)));

                // Second tasks should wait when locks will be released
                await Task.Delay(100);

                Assert.False(a2.IsCompleted, Message(nameof(a2), nameof(a1)));
                Assert.False(b2.IsCompleted, Message(nameof(b2), nameof(b1)));
                Assert.False(c2.IsCompleted, Message(nameof(c2), nameof(c1)));

                Assert.False(a3.IsCompleted, Message(nameof(a3), nameof(a1)));
                Assert.False(b3.IsCompleted, Message(nameof(b3), nameof(b1)));
                Assert.False(c3.IsCompleted, Message(nameof(c3), nameof(c1)));

                // When first tasks release locks, second tasks can continue
                await a1.Result.DisposeAsync();
                await b1.Result.DisposeAsync();
                await c1.Result.DisposeAsync();

                await Task.WhenAll(a2, b2, c2);

                Assert.True(a2.IsCompleted, Message(nameof(a2)));
                Assert.True(b2.IsCompleted, Message(nameof(b2)));
                Assert.True(c2.IsCompleted, Message(nameof(c2)));

                Assert.False(a3.IsCompleted, Message(nameof(a3), nameof(a2)));
                Assert.False(b3.IsCompleted, Message(nameof(b3), nameof(b2)));
                Assert.False(c3.IsCompleted, Message(nameof(c3), nameof(c2)));

                await a2.Result.DisposeAsync();
                await b2.Result.DisposeAsync();
                await c2.Result.DisposeAsync();


                await Task.WhenAll(a3, b3, c3);

                Assert.True(a3.IsCompleted, Message(nameof(a3)));
                Assert.True(b3.IsCompleted, Message(nameof(b3)));
                Assert.True(c3.IsCompleted, Message(nameof(c3)));

                await a3.Result.DisposeAsync();
                await b3.Result.DisposeAsync();
                await c3.Result.DisposeAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private string Message(string x) => $"Task {x} should be completed.";

        private string Message(string x, string y) => $"Task {x} did not wait for task {y} to release the distributed lock.";
    }
}
