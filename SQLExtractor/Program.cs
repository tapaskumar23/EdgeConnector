using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Exceptions;
using Microsoft.Health.SQL.Extractor.Extensions;
using Microsoft.Health.SQL.Extractor.Extractor;
using Microsoft.Health.SQL.Extractor.SQLData;

public class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
            

        builder.Services.AddLogging();            
        builder.Services.AddSingleton<ISqlConnectorFactory, SqlConnectorFactory>();
        builder.Services.AddSingleton(TimeProvider.System);

        int endpointCount = 0;
        builder.Services.AddLocalStorageEndpoint(configuration, ref endpointCount);

        if (endpointCount > 1)
        {
            throw new InvalidConfigurationException("The application can only have 1 endpoint configured - LocalStorageEndpointConfiguration.");
        }

        if (endpointCount == 0)
        {
            throw new InvalidConfigurationException("No valid endpoint configuration was provided.");
        }

        IHost host = builder.Build();
        host.Run();
    }
}
