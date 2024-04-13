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

        const string lockKeyA = "A";
        const string lockKeyB = "B";
        const string lockKeyC = "C";

        for (var i = 0; i < 5; i++)
        {
            var a1 = inMemoryLock.Acquire(lockKeyA);
            var b1 = inMemoryLock.Acquire(lockKeyB);
            var c1 = inMemoryLock.Acquire(lockKeyC);

            var a2 = inMemoryLock.Acquire(lockKeyA);
            var b2 = inMemoryLock.Acquire(lockKeyB);
            var c2 = inMemoryLock.Acquire(lockKeyC);

            var a3 = inMemoryLock.Acquire(lockKeyA);
            var b3 = inMemoryLock.Acquire(lockKeyB);
            var c3 = inMemoryLock.Acquire(lockKeyC);

            await Task.WhenAll(a1, b1, c1);

            Assert.True(a1.IsCompleted, Message(nameof(a1)));
            Assert.True(b1.IsCompleted, Message(nameof(b1)));
            Assert.True(c1.IsCompleted, Message(nameof(c1)));

            // Second tasks should wait when locks will be released
            Assert.False(a2.IsCompleted, Message(nameof(a2), nameof(a1)));
            Assert.False(b2.IsCompleted, Message(nameof(b2), nameof(b1)));
            Assert.False(c2.IsCompleted, Message(nameof(c2), nameof(c1)));

            Assert.False(a3.IsCompleted, Message(nameof(a3), nameof(a1)));
            Assert.False(b3.IsCompleted, Message(nameof(b3), nameof(b1)));
            Assert.False(c3.IsCompleted, Message(nameof(c3), nameof(c1)));

            // When first tasks release locks, second tasks can continue
            a1.Result.Dispose();
            b1.Result.Dispose();
            c1.Result.Dispose();

            await Task.WhenAll(a2, b2, c2);

            Assert.True(a2.IsCompleted, Message(nameof(a2)));
            Assert.True(b2.IsCompleted, Message(nameof(b2)));
            Assert.True(c2.IsCompleted, Message(nameof(c2)));

            Assert.False(a3.IsCompleted, Message(nameof(a3), nameof(a2)));
            Assert.False(b3.IsCompleted, Message(nameof(b3), nameof(b2)));
            Assert.False(c3.IsCompleted, Message(nameof(c3), nameof(c2)));

            a2.Result.Dispose();
            b2.Result.Dispose();
            c2.Result.Dispose();

            await Task.WhenAll(a3, b3, c3);

            Assert.True(a3.IsCompleted, Message(nameof(a3)));
            Assert.True(b3.IsCompleted, Message(nameof(b3)));
            Assert.True(c3.IsCompleted, Message(nameof(c3)));

            a3.Result.Dispose();
            b3.Result.Dispose();
            c3.Result.Dispose();
        }
    }

    private string Message(string x) => $"Task {x} should be completed.";

    private string Message(string x, string y) => $"Task {x} did not wait for task {y} to release the distributed lock.";
}