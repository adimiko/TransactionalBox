using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression.GZip.Internals;

namespace TransactionalBox.OutboxWorker.Compression.GZip.Settings
{
    public sealed class GZipCompressionSettings : IGZipCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal GZipCompressionSettings() { }
    }
}
