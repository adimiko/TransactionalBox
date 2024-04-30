using System.IO.Compression;
using TransactionalBox.Outbox.Compression.Brotli.Internals;

namespace TransactionalBox.Outbox.Compression.Brotli.Settings
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
