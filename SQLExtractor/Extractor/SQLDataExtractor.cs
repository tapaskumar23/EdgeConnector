using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public class SQLDataExtractor<TSQLDataContext> : ISQLDataExtractor<TSQLDataContext>
            where TSQLDataContext : SQLDataContext
    {
        private readonly ILogger<SQLDataExtractor<TSQLDataContext>> _logger;
        private readonly ISqlConnectorFactory _sqlConnectorFactory;
        private readonly ISQLDataContextFactory<TSQLDataContext> _sqlDataContextFactory;
        private readonly IOptions<SQLConnectorConfiguration> _sQLConnectorConfiguration;

        public event Func<SQLDataContext, Task> OnDataExtracted;

        event Func<TSQLDataContext, Task> ISQLDataExtractor<TSQLDataContext>.OnDataExtracted
        {
            add
            {
               
            }

            remove
            {
               
            }
        }

        public SQLDataExtractor(
            ILogger<SQLDataExtractor<TSQLDataContext>> logger,
            ISqlConnectorFactory sqlConnectorFactory,
            IOptions<SQLConnectorConfiguration> configuration,
            ISQLDataContextFactory<TSQLDataContext> sqlDataContextFactory)
        {
            _logger = logger;
            _sqlConnectorFactory = sqlConnectorFactory;
            _sqlDataContextFactory = sqlDataContextFactory;
            _sQLConnectorConfiguration = configuration;
        }

        public Task ExtractData(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PerformInitalSetup(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifySQLConnection(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_sqlConnectorFactory.TestConnection(_sQLConnectorConfiguration.Value.Server,
                _sQLConnectorConfiguration.Value.Database,
                _sQLConnectorConfiguration.Value.Username,
                _sQLConnectorConfiguration.Value.Password));
        }
    }
}
