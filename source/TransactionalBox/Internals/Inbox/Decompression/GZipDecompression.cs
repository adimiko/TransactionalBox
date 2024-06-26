﻿using Microsoft.IO;
using System.IO.Compression;

namespace TransactionalBox.Internals.Inbox.Decompression
{
    internal sealed class GZipDecompression : IDecompression
    {
        public string Name { get; } = "gzip";

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
                await gZipStream.CopyToAsync(memoryStreamOutput).ConfigureAwait(false);

                await gZipStream.FlushAsync().ConfigureAwait(false);

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
