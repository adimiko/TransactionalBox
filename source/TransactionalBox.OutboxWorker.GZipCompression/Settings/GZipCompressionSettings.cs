using System.IO.Compression;
using TransactionalBox.OutboxWorker.GZipCompression.Internals;

namespace TransactionalBox.OutboxWorker.GZipCompression.Settings
{
    public sealed class GZipCompressionSettings : IGZipCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal GZipCompressionSettings() { }
    }
}
