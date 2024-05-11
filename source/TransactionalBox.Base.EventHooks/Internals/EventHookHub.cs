using System.Threading.Channels;

namespace TransactionalBox.Base.EventHooks.Internals
{
    internal sealed class EventHookHub<TEventHook> where TEventHook : EventHook, new()
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

        public EventHookHub(TimeProvider timeProvider) => _timeProvider = timeProvider;

        public ValueTask PublishAsync() => _writer.WriteAsync(_timeProvider.GetUtcNow().UtcDateTime);

        public IAsyncEnumerable<DateTime> ListenAsync(CancellationToken cancellationToken) => _reader.ReadAllAsync(cancellationToken);
    }
}
