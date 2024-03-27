using System.IO.Compression;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.GZipDecompression.Internals
{
    internal sealed class GZipDecompression : IDecompressionAlgorithm
    {
        public byte[] Decompress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (GZipStream gZipStream = new GZipStream(memoryStreamInput, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(memoryStreamOutput);

                memoryStreamOutput.Seek(0, SeekOrigin.Begin);

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
