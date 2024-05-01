using System.Threading.Channels;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal interface IInMemoryTransport
    {
        ChannelWriter<TransportObject> Writer { get; }

        ChannelReader<TransportObject> Reader { get; }
    }
}
