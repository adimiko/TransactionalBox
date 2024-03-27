using System.IO.Compression;
using TransactionalBox.OutboxWorker.Internals.Compression;

namespace TransactionalBox.OutboxWorker.Settings.Compression
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
