using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.Internals.Compression
{
    internal sealed class BrotliCompression : ICompressionAlgorithm
    {
        public byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(memoryStreamOutput, CompressionMode.Compress))
            {
                memoryStreamInput.CopyTo(brotliStream);

                brotliStream.Close();

                byte[] output = memoryStreamOutput.ToArray();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
