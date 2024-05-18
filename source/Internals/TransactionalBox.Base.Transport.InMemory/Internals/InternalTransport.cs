using System.Threading.Channels;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal sealed class InternalTransport : IInMemoryTransport
    {
        private static readonly Channel<TransportObject> _channel = Channel.CreateUnbounded<TransportObject>();

        public ChannelWriter<TransportObject> Writer { get; } = _channel.Writer;

        public ChannelReader<TransportObject> Reader { get; } = _channel.Reader;
    }
}
