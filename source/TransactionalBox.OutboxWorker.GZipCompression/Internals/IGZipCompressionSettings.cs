using System.IO.Compression;

namespace TransactionalBox.OutboxWorker.GZipCompression.Internals
{
    internal interface IGZipCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
