using System.Threading.Channels;

namespace TransactionalBox.Internals.Transport.InMemory.Internals
{
    internal interface IInMemoryTransport
    {
        ChannelWriter<TransportObject> Writer { get; }

        ChannelReader<TransportObject> Reader { get; }
    }
}
