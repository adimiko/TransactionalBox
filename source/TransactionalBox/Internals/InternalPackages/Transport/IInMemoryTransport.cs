using System.Threading.Channels;

namespace TransactionalBox.Internals.InternalPackages.Transport
{
    internal interface IInMemoryTransport
    {
        ChannelWriter<TransportObject> Writer { get; }

        ChannelReader<TransportObject> Reader { get; }
    }
}
