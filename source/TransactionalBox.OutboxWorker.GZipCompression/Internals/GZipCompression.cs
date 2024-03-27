using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.GZipCompression.Internals
{
    internal sealed class GZipCompression : ICompressionAlgorithm
    {
        private readonly IGZipCompressionSettings _settings;

        public GZipCompression(IGZipCompressionSettings settings)
        {
            _settings = settings;
        }

        public byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (GZipStream gZipStream = new GZipStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                memoryStreamInput.CopyTo(gZipStream);

                gZipStream.Close();

                byte[] output = memoryStreamOutput.ToArray();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
