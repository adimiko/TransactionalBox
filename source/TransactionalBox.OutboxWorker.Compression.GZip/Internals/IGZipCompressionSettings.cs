using System.IO.Compression;

namespace TransactionalBox.OutboxWorker.Compression.GZip.Internals
{
    internal interface IGZipCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
