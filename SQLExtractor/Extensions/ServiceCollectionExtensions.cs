

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage;
using Microsoft.Health.SQL.Extractor.Endpoints;
using Microsoft.Health.SQL.Extractor.Exceptions;
using Microsoft.Health.SQL.Extractor.SQLData;
using Microsoft.Health.SQL.Extractor.Extractor;

namespace Microsoft.Health.SQL.Extractor.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddLocalStorageEndpoint(this IServiceCollection services, 
            IConfiguration configuration, ref int endpointCount)
        {
            var localStorageEndpointConfiguration = new LocalStorageEndpointConfiguration();
            configuration.Bind(nameof(LocalStorageEndpointConfiguration), localStorageEndpointConfiguration);

            bool isConfigurationValid = true;

            if(localStorageEndpointConfiguration.IsValid())
            {
                if (!Directory.Exists(localStorageEndpointConfiguration.Path))
                {
                    isConfigurationValid = false;
                    throw new InvalidConfigurationException("The path provided for the local storage endpoint does not exist.");
                }

                services.AddOptions<LocalStorageEndpointConfiguration>()
                .Bind(configuration.GetSection(nameof(LocalStorageEndpointConfiguration)))
                .Validate(configuration => configuration.IsValid())
                .ValidateOnStart();
            }

            var sqlExtractorConfiguration = new SQLExtractorConfiguration();
            configuration.Bind(nameof(SQLExtractorConfiguration), localStorageEndpointConfiguration);

            if (sqlExtractorConfiguration.IsValid())
            {
                if (string.IsNullOrEmpty(sqlExtractorConfiguration.Server) || string.IsNullOrEmpty(sqlExtractorConfiguration.Database)
                    || string.IsNullOrEmpty(sqlExtractorConfiguration.Username) || string.IsNullOrEmpty(sqlExtractorConfiguration.Password))
                {
                    isConfigurationValid = false;
                    throw new InvalidConfigurationException("The connection string to on-prem database for the local storage endpoint is not correct.");
                }

                services.AddOptions<SQLExtractorConfiguration>()
                .Bind(configuration.GetSection(nameof(SQLExtractorConfiguration)))
                .Validate(configuration => configuration.IsValid())
                .ValidateOnStart();
            }

            if(isConfigurationValid)
            {
                services.AddSingleton<ISQLDataExtractor<SQLDataContext>, ISQLDataExtractor<SQLDataContext>>();
                services.AddSingleton<IExternalEndpoint<LocalStorageSQLDataContext>, LocalStorageEndpoint>();
                services.AddSingleton<ISQLDataContextFactory<LocalStorageSQLDataContext>, LocalStorageSQLDataContextFactory>();
                services.AddHostedService<Worker<SQLDataContext>>();
                                           
                endpointCount++;

            }

            return services;
        }
    }
}
