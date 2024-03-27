using System.IO.Compression;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.Internals.Decompression
{
    internal sealed class BrotliDecompression : IDecompressionAlgorithm
    {
        public byte[] Decompress(byte[] input)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(input))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(memoryStreamInput, CompressionMode.Decompress))
            {
                brotliStream.CopyTo(memoryStreamOutput);

                memoryStreamOutput.Seek(0, SeekOrigin.Begin);

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
