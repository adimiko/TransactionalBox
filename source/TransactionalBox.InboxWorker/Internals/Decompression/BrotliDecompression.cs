using System.IO.Compression;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.Internals.Decompression
{
    internal sealed class BrotliDecompression : IDecompressionAlgorithm
    {
        public async Task<byte[]> Decompress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(memoryStreamInput, CompressionMode.Decompress))
            {
                await brotliStream.CopyToAsync(memoryStreamOutput);

                memoryStreamOutput.Seek(0, SeekOrigin.Begin);

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
