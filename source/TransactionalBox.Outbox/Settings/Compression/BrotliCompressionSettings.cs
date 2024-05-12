using System.IO.Compression;
using TransactionalBox.Outbox.Internals.Compression.Brotli;

namespace TransactionalBox.Outbox.Settings.Compression
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
