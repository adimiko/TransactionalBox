using System.IO.Compression;
using TransactionalBox.Outbox.Internals.Compression.GZip;

namespace TransactionalBox.Outbox.Settings.Compression
{
    public sealed class GZipCompressionSettings : IGZipCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal GZipCompressionSettings() { }
    }
}
