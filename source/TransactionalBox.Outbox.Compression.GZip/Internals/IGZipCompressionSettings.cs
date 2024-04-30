using System.IO.Compression;

namespace TransactionalBox.Outbox.Compression.GZip.Internals
{
    internal interface IGZipCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
