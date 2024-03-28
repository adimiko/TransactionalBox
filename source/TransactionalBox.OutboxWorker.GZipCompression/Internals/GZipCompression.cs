using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.GZipCompression.Internals
{
    internal sealed class GZipCompression : ICompressionAlgorithm
    {
        private readonly IGZipCompressionSettings _settings;

        public GZipCompression(IGZipCompressionSettings settings)
        {
            _settings = settings;
        }

        public async Task<byte[]> Compress(byte[] data)
        {
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (GZipStream gZipStream = new GZipStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                await gZipStream.WriteAsync(data, 0, data.Length);
                await gZipStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
