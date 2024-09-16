using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage
{
    public class LocalStorageEndpoint : ILocalStorageEndpoint
    {
        private readonly string _path;
        private readonly ILogger<LocalStorageEndpoint> _logger;

        public LocalStorageEndpoint(
            IOptions<LocalStorageEndpointConfiguration> configuration,
            ILogger<LocalStorageEndpoint> logger)
        {
            _path = EnsureArg.IsNotNullOrWhiteSpace(configuration?.Value?.Path, nameof(configuration.Value.Path));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }

        public Task<(string code, string error)?> Send(LocalStorageSQLDataContext context, DataTable sqlData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
