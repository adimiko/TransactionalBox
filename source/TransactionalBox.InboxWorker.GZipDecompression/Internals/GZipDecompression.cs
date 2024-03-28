using System.IO.Compression;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.GZipDecompression.Internals
{
    internal sealed class GZipDecompression : IDecompressionAlgorithm
    {
        public async Task<byte[]> Decompress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (GZipStream gZipStream = new GZipStream(memoryStreamInput, CompressionMode.Decompress))
            {
                await gZipStream.CopyToAsync(memoryStreamOutput);

                await gZipStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
