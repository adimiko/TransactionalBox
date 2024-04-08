﻿using Microsoft.IO;
using System.IO.Compression;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.Compression.Brotli.Internals
{
    internal sealed class BrotliCompression : ICompressionAlgorithm
    {
        private readonly IBrotliCompressionSettings _settings;

        private readonly RecyclableMemoryStreamManager _streamManager;

        public BrotliCompression(
            IBrotliCompressionSettings settings,
            RecyclableMemoryStreamManager streamManager)
        {
            _settings = settings;
            _streamManager = streamManager;
        }

        public async Task<byte[]> Compress(byte[] data)
        {
            using (var memoryStreamOutput = _streamManager.GetStream())
            using (var brotliStream = new BrotliStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                await brotliStream.WriteAsync(data, 0, data.Length);
                await brotliStream.FlushAsync();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}