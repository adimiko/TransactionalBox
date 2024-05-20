﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;

namespace TransactionalBox
{
    public static class ExtensionUseGZipDecompression
    {
        public static void UseGZipDecompression(this IInboxDecompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompression, GZipDecompression>();
        }
    }
}
