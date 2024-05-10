using System.Threading.Channels;
using TransactionalBox.Internals;

namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal sealed class HookHub<THook> : IHookCaller<THook>, IHookListener<THook>
        where THook : Hook
    {
        private static Channel<DateTime> _channel = Channel.CreateBounded<DateTime>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false, //TODO
        });

        private ChannelWriter<DateTime> _writer => _channel.Writer;

        private ChannelReader<DateTime> _reader => _channel.Reader;

        private readonly ISystemClock _systemClock;

        public HookHub(ISystemClock systemClock) => _systemClock = systemClock;

        public ValueTask CallAsync() => _writer.WriteAsync(_systemClock.UtcNow);

        public IAsyncEnumerable<DateTime> ListenAsync(CancellationToken cancellationToken) => _reader.ReadAllAsync(cancellationToken);
    }
}
