using Microsoft.IO;
using System.IO.Compression;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.Inbox.Decompression.GZip.Internals
{
    internal sealed class GZipDecompression : IDecompressionAlgorithm
    {
        private readonly RecyclableMemoryStreamManager _streamManager;

        public GZipDecompression(RecyclableMemoryStreamManager streamManager)
        {
            _streamManager = streamManager;
        }

        public async Task<byte[]> Decompress(byte[] data)
        {
            using (var memoryStreamInput = _streamManager.GetStream(data))
            using (var memoryStreamOutput = _streamManager.GetStream())
            using (var gZipStream = new GZipStream(memoryStreamInput, CompressionMode.Decompress))
            {
                await gZipStream.CopyToAsync(memoryStreamOutput);

                await gZipStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
