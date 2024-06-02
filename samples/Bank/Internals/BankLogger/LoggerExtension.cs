using System.Reflection;
using Serilog.Events;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BankLogger
{
    public static class LoggerExtension
    {
        public static void AddBankLogger(this WebApplicationBuilder builder, IConfiguration configuration, Assembly assembly)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
            {
                AutoRegisterTemplate = true,
                //AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                IndexFormat = $"{assembly.GetName().Name!.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            })
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
