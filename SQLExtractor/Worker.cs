using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Endpoints;
using Microsoft.Health.SQL.Extractor.Extractor;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor
{

    public sealed class Worker<TSQLDataContext> : BackgroundService
        where TSQLDataContext : SQLDataContext
    {
        private readonly ISQLDataExtractor<TSQLDataContext> _sqlDataExtractor;
        private readonly IExternalEndpoint<TSQLDataContext> _externalEndpoint;
        private readonly ILogger<Worker<TSQLDataContext>> _logger;
        private readonly IOptions<SQLConnectorConfiguration> _sQLConnectorConfiguration;
        public Worker(
                        ISQLDataExtractor<TSQLDataContext> sqlDataExtractor,
                        IExternalEndpoint<TSQLDataContext> externalEndpoint,
                        IOptions<SQLConnectorConfiguration> sQLConnectorConfiguration,
            ILogger<Worker<TSQLDataContext>> logger)
        {
            _externalEndpoint = EnsureArg.IsNotNull(externalEndpoint, nameof(externalEndpoint));
            _sqlDataExtractor = EnsureArg.IsNotNull(sqlDataExtractor, nameof(sqlDataExtractor));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _sQLConnectorConfiguration = EnsureArg.IsNotNull(sQLConnectorConfiguration, nameof(sQLConnectorConfiguration));
            _sqlDataExtractor.OnDataExtracted += OnDataExtracted;
        }

        private Task OnDataExtracted(TSQLDataContext arg)
        {
            _sqlDataExtractor.ExtractData(default);
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var utcNow = DateTime.UtcNow;

                await Task.Delay(_sQLConnectorConfiguration.Value.InternalInMins * 1000, stoppingToken);

                await _externalEndpoint.PerformInitialSetupAsync(_sQLConnectorConfiguration.Value.ServerName, _sQLConnectorConfiguration.Value.Database);
                //Verify the Connection

                await _sqlDataExtractor.VerifySQLConnection(stoppingToken).ConfigureAwait(false);
                await _sqlDataExtractor.PerformInitalSetup(stoppingToken).ConfigureAwait(false);

                //Perform the initial Setup
                await _sqlDataExtractor.ExtractData(stoppingToken).ConfigureAwait(false);
            }


        }

        public override void Dispose()
        {
            _sqlDataExtractor.OnDataExtracted -= OnDataExtracted;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
