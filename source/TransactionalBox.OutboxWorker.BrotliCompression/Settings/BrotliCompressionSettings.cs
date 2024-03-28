using System.IO.Compression;
using TransactionalBox.OutboxWorker.BrotliCompression.Internals;

namespace TransactionalBox.OutboxWorker.BrotliCompression.Settings
{
    public sealed class BrotliCompressionSettings : IBrotliCompressionSettings
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

        internal BrotliCompressionSettings() { }
    }
}
