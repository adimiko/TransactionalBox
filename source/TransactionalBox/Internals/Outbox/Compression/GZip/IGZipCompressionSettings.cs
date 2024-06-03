using System.IO.Compression;

namespace TransactionalBox.Internals.Outbox.Compression.GZip
{
    internal interface IGZipCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
