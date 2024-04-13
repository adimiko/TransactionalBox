using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TransactionalBox.KeyedInMemoryLock.Tests;

public sealed class KeyedInMemoryLockTest
{
    private readonly IServiceProvider _serviceProvider = new ServiceCollection().AddKeyedInMemoryLock().BuildServiceProvider();

    [Fact(DisplayName = "KeyedInMemoryLock works correct in multi-thread enviroment")]
    public async Task Test()
    {
        var inMemoryLock = _serviceProvider.GetRequiredService<IKeyedInMemoryLock>();

        for (var i = 0; i < 5; i++)
        {
            // Different lock keys
            var lockKeyA = "A";
            var lockKeyB = "B";
            var lockKeyC = "C";

            var taskA1 = inMemoryLock.Acquire(lockKeyA);
            var taskB1 = inMemoryLock.Acquire(lockKeyB);
            var taskC1 = inMemoryLock.Acquire(lockKeyC);

            var taskA2 = inMemoryLock.Acquire(lockKeyA);
            var taskB2 = inMemoryLock.Acquire(lockKeyB);
            var taskC2 = inMemoryLock.Acquire(lockKeyC);

            var taskA3 = inMemoryLock.Acquire(lockKeyA);
            var taskB3 = inMemoryLock.Acquire(lockKeyB);
            var taskC3 = inMemoryLock.Acquire(lockKeyC);

            await Task.WhenAll(taskA1, taskB1, taskC1);

            Assert.True(taskA1.IsCompleted);
            Assert.True(taskB1.IsCompleted);
            Assert.True(taskC1.IsCompleted);

            // Second tasks should wait when locks will be released
            await Task.Delay(25);

            Assert.False(taskA2.IsCompleted);
            Assert.False(taskB2.IsCompleted);
            Assert.False(taskC2.IsCompleted);

            Assert.False(taskA3.IsCompleted);
            Assert.False(taskB3.IsCompleted);
            Assert.False(taskC3.IsCompleted);

            // When first tasks release locks, second tasks can continue
            taskA1.Result.Dispose();
            taskB1.Result.Dispose();
            taskC1.Result.Dispose();

            await Task.WhenAll(taskA2, taskB2, taskC2);

            Assert.True(taskA2.IsCompleted);
            Assert.True(taskB2.IsCompleted);
            Assert.True(taskC2.IsCompleted);

            Assert.False(taskA3.IsCompleted);
            Assert.False(taskB3.IsCompleted);
            Assert.False(taskC3.IsCompleted);

            taskA2.Result.Dispose();
            taskB2.Result.Dispose();
            taskC2.Result.Dispose();

            await Task.WhenAll(taskA3, taskB3, taskC3);

            Assert.True(taskA3.IsCompleted);
            Assert.True(taskB3.IsCompleted);
            Assert.True(taskC3.IsCompleted);

            taskA3.Result.Dispose();
            taskB3.Result.Dispose();
            taskC3.Result.Dispose();
        }
    }
}