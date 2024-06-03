using System.IO.Compression;
using TransactionalBox.Internals.Outbox.Compression.Brotli;

namespace TransactionalBox.Settings.Outbox.Compression
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
