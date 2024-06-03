using System.IO.Compression;
using TransactionalBox.Internals.Outbox.Compression.GZip;

namespace TransactionalBox.Settings.Outbox.Compression
{
    public sealed class GZipCompressionSettings : IGZipCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal GZipCompressionSettings() { }
    }
}
