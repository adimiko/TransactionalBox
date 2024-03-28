using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.BrotliCompression.Internals
{
    internal sealed class BrotliCompression : ICompressionAlgorithm
    {
        private readonly IBrotliCompressionSettings _settings;

        public BrotliCompression(IBrotliCompressionSettings settings)
        {
            _settings = settings;
        }

        public async Task<byte[]> Compress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                await memoryStreamInput.CopyToAsync(brotliStream);

                brotliStream.Close();

                byte[] output = memoryStreamOutput.ToArray();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
