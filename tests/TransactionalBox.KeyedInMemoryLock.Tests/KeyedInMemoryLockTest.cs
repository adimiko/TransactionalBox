using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TransactionalBox.KeyedInMemoryLock.Tests;

public sealed class KeyedInMemoryLockTest
{
    private readonly IServiceProvider _serviceProvider;

    public KeyedInMemoryLockTest() 
    {
        IServiceCollection services = new ServiceCollection();

        services.AddKeyedInMemoryLock();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact(DisplayName = "KeyedInMemoryLock works correct in multi-thread enviroment")]
    public async Task Test()
    {
        for (var i = 0; i < 2; i++)
        {
            // Different lock keys
            var lockKeyA = "A";
            var lockKeyB = "B";

            // Create different instances for each scope
            var lockForTaskA1 = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();
            var lockForTaskB1 = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();

            var lockForTaskA2 = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();
            var lockForTaskB2 = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();

            var taskA1 = lockForTaskA1.Acquire(lockKeyA);
            var taskB1 = lockForTaskB1.Acquire(lockKeyB);

            var taskA2 = lockForTaskA2.Acquire(lockKeyA);
            var taskB2 = lockForTaskB2.Acquire(lockKeyB);

            await Task.WhenAll(taskA1, taskB1);

            Assert.True(taskA1.IsCompleted);
            Assert.True(taskB1.IsCompleted);

            // Second tasks should wait when locks will be released
            Assert.False(taskA2.IsCompleted);
            Assert.False(taskB2.IsCompleted);

            // When first tasks release locks, second tasks can continue
            lockForTaskA1.Release();
            lockForTaskB1.Release();

            await Task.WhenAll(taskA2, taskB2);

            Assert.True(taskA2.IsCompleted);
            Assert.True(taskB2.IsCompleted);

            lockForTaskA2.Release();
            lockForTaskB2.Release();
        }
    }
}