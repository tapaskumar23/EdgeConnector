
using EnsureThat;
using FellowOakDicom;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Health.DICOM.Listener.Configuration;
using Microsoft.Health.DICOM.Listener.Extensions;
using Microsoft.Health.DICOM.Listener.Listener;


namespace Microsoft.Health.DICOM.Listener
{
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

            builder.Services.AddOptions<DICOMFileListenerConfiguration>()
                .Bind(configuration.GetSection(nameof(DICOMFileListenerConfiguration)))
                .Validate(configuration => configuration.IsValid(), "Port must be between 1 and 65535")
                .ValidateOnStart();

            // builder.Services.AddHttpClient();
            builder.Services.AddLogging();
            builder.Services.AddSingleton<ITcpListenerFactory, TcpListenerFactory>();
            builder.Services.AddSingleton(TimeProvider.System);

            // Use DicomSetupBuilder to configure the internals of Fellow Oak DICOM
            new DicomSetupBuilder().RegisterServices(s => s.AddFellowOakDicom()).Build();

            int endpointCount = 0;
            builder.Services.AddLocalStorageEndpoint(configuration, ref endpointCount);

            if (endpointCount > 1)
            {
                throw new InvalidConfigurationException("The application can only have 1 endpoint configured - HttpEndpointConfiguration, MqttEndpointConfiguration OR LocalStorageEndpointConfiguration.");
            }

            if (endpointCount == 0)
            {
                throw new InvalidConfigurationException("No valid endpoint configuration was provided.");
            }

            IHost host = builder.Build();
            host.Run();
        }
    }
}