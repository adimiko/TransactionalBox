using System.Threading.Channels;

namespace TransactionalBox.Base.Hooks.Internals
{
    internal sealed class HookHub<THook> : IHookCaller<THook>
        where THook : Hook, new()
    {
        private static Channel<DateTime> _channel = Channel.CreateBounded<DateTime>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
        });

        private ChannelWriter<DateTime> _writer => _channel.Writer;

        private ChannelReader<DateTime> _reader => _channel.Reader;

        private readonly TimeProvider _timeProvider;

        public HookHub(TimeProvider timeProvider) => _timeProvider = timeProvider;

        public ValueTask CallAsync() => _writer.WriteAsync(_timeProvider.GetUtcNow().UtcDateTime);

        public IAsyncEnumerable<DateTime> ListenAsync(CancellationToken cancellationToken) => _reader.ReadAllAsync(cancellationToken);
    }
}
