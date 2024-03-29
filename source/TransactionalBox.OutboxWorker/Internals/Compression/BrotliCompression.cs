﻿using System.IO.Compression;
using System.Net;
using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.Internals.Compression
{
    internal sealed class BrotliCompression : ICompressionAlgorithm
    {
        private readonly IBrotliCompressionSettings _settings;

        public BrotliCompression(IBrotliCompressionSettings settings) 
        {
            _settings = settings;
        }

        public byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStreamInput = new MemoryStream(data))
            using (MemoryStream memoryStreamOutput = new MemoryStream())
            using (BrotliStream brotliStream = new BrotliStream(memoryStreamOutput, _settings.CompressionLevel))
            {
                memoryStreamInput.CopyTo(brotliStream);

                brotliStream.Close();

                byte[] output = memoryStreamOutput.ToArray();

                return memoryStreamOutput.ToArray();
            }
        }
    }
}
