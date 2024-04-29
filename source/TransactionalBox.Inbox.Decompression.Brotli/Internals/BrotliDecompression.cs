using Microsoft.IO;
using System.IO.Compression;

namespace TransactionalBox.Inbox.Decompression.Brotli.Internals
{
    internal sealed class BrotliDecompression : IDecompressionAlgorithm
    {
        private readonly RecyclableMemoryStreamManager _streamManager;

        public BrotliDecompression(
            RecyclableMemoryStreamManager streamManager)
        {
            _streamManager = streamManager;
        }

        public async Task<byte[]> Decompress(byte[] data)
        {
            using (var memoryStreamInput = _streamManager.GetStream(data))
            using (var memoryStreamOutput = _streamManager.GetStream())
            using (var brotliStream = new BrotliStream(memoryStreamInput, CompressionMode.Decompress))
            {
                await brotliStream.CopyToAsync(memoryStreamOutput);

                await brotliStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
