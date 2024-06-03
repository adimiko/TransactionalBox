using Microsoft.IO;
using System.IO.Compression;

namespace TransactionalBox.Internals.Inbox.Decompression
{
    internal sealed class BrotliDecompression : IDecompression
    {
        public string Name { get; } = "brotli";

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
