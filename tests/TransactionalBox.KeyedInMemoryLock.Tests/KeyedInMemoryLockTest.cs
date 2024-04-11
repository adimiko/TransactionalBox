using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TransactionalBox.KeyedInMemoryLock.Tests;

public sealed class KeyedInMemoryLockTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection().AddKeyedInMemoryLock().BuildServiceProvider();

    [Fact(DisplayName = "KeyedInMemoryLock works correct in multi-thread enviroment")]
    public async Task Test()
    {
        for (var i = 0; i < 2; i++)
        {
            // Different lock keys
            var lockKeyA = "A";
            var lockKeyB = "B";

            var inMemoryLock = _serviceProvider.GetRequiredService<IKeyedInMemoryLock>();

            var taskA1 = inMemoryLock.Acquire(lockKeyA);
            var taskB1 = inMemoryLock.Acquire(lockKeyB);

            var taskA2 = inMemoryLock.Acquire(lockKeyA);
            var taskB2 = inMemoryLock.Acquire(lockKeyB);

            await Task.WhenAll(taskA1, taskB1);

            Assert.True(taskA1.IsCompleted);
            Assert.True(taskB1.IsCompleted);

            // Second tasks should wait when locks will be released
            Assert.False(taskA2.IsCompleted);
            Assert.False(taskB2.IsCompleted);

            // When first tasks release locks, second tasks can continue
            taskA1.Result.Dispose();
            taskB1.Result.Dispose();

            await Task.WhenAll(taskA2, taskB2);

            Assert.True(taskA2.IsCompleted);
            Assert.True(taskB2.IsCompleted);

            taskA2.Result.Dispose();
            taskB2.Result.Dispose();
        }
    }
}