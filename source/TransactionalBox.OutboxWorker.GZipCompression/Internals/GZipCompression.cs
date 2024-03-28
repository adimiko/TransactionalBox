using Microsoft.IO;
using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.GZipCompression.Internals
{
    internal sealed class GZipCompression : ICompressionAlgorithm
    {
        private readonly IGZipCompressionSettings _settings;

        private readonly RecyclableMemoryStreamManager _streamManager;

        public GZipCompression(
            IGZipCompressionSettings settings,
            RecyclableMemoryStreamManager streamManager)
        {
            _settings = settings;
            _streamManager = streamManager;
        }

        public async Task<byte[]> Compress(byte[] data)
        {
            using (var memoryStreamOutput = _streamManager.GetStream())
            using (var gZipStream = new GZipStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                await gZipStream.WriteAsync(data, 0, data.Length);
                await gZipStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
