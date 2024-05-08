using Microsoft.IO;
using System.IO.Compression;
using TransactionalBox.Inbox.Internals.Decompression;

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
                await brotliStream.CopyToAsync(memoryStreamOutput).ConfigureAwait(false);

                await brotliStream.FlushAsync().ConfigureAwait(false);

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
