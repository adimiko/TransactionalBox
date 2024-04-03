using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression.Brotli.Internals;

namespace TransactionalBox.OutboxWorker.Compression.Brotli.Settings
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
