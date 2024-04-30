using System.IO.Compression;
using TransactionalBox.Outbox.Compression.GZip.Internals;

namespace TransactionalBox.Outbox.Compression.GZip.Settings
{
    public sealed class GZipCompressionSettings : IGZipCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal GZipCompressionSettings() { }
    }
}
