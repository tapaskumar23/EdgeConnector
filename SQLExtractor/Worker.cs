using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public Worker(
                        ISQLDataExtractor<TSQLDataContext> sqlDataExtractor,
                        IExternalEndpoint<TSQLDataContext> externalEndpoint,
            ILogger<Worker<TSQLDataContext>> logger)
        {
            _externalEndpoint = EnsureArg.IsNotNull(externalEndpoint, nameof(externalEndpoint));
            _sqlDataExtractor = EnsureArg.IsNotNull(sqlDataExtractor, nameof(sqlDataExtractor));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _sqlDataExtractor.OnDataExtracted += OnDataExtracted;
        }

        private Task OnDataExtracted(TSQLDataContext arg)
        {
            _sqlDataExtractor.ExtractData(default);
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Verify the Connection

            await _sqlDataExtractor.VerifySQLConnection(stoppingToken).ConfigureAwait(false);

            //Perform the initial Setup
            await _sqlDataExtractor.ExtractData(stoppingToken).ConfigureAwait(false);
        }

        public override void Dispose()
        {
            _sqlDataExtractor.OnDataExtracted -= OnDataExtracted;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
