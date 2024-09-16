using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        private readonly ISQLDataContextFactory<TSQLDataContext> _sqlDataContextFactory;
        private readonly ISQLDataExtractor<TSQLDataContext> _sqlDataExtractor;
        private readonly ILogger<Worker<TSQLDataContext>> _logger;
        public Worker(
            ISQLDataContextFactory<TSQLDataContext> sqlDataContextFactory,
            ISQLDataExtractor<TSQLDataContext> sqlDataExtractor,
            ILogger<Worker<TSQLDataContext>> logger)
        {
            _sqlDataContextFactory = EnsureArg.IsNotNull(sqlDataContextFactory, nameof(sqlDataContextFactory));
            _sqlDataExtractor = EnsureArg.IsNotNull(sqlDataExtractor, nameof(sqlDataExtractor));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _sqlDataExtractor.OnDataExtracted += OnDataExtracted;
        }

        private Task OnDataExtracted(TSQLDataContext arg)
        {
            //_sqlDataExtractor.ExtractData(default);
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            _sqlDataExtractor.OnDataExtracted -= OnDataExtracted;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
