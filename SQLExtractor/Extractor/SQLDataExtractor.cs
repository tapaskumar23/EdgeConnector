using Microsoft.Extensions.Logging;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public class SQLDataExtractor<TSQLDataContext> : ISQLDataExtractor<TSQLDataContext> where TSQLDataContext: SQLDataContext
    {
        private readonly ILogger<SQLDataExtractor<TSQLDataContext>> _logger;
        private readonly ISqlConnectorFactory<TSQLDataContext> _sqlConnectorFactory;
        private readonly SQLExtractorConfiguration _configuration;
        private readonly ISQLDataContextFactory<TSQLDataContext> _sqlDataContextFactory;


        public event Func<SQLDataContext, Task> OnDataExtracted;

        event Func<TSQLDataContext, Task> ISQLDataExtractor<TSQLDataContext>.OnDataExtracted
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public Task ExtractData(CancellationToken cancellationToken)
        {



            throw new NotImplementedException();
        }

        public Task PerformInitalSetup(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task VerifySQLConnection(CancellationToken cancellationToken)
        {
            //Create a connection and test the connetion 

            throw new NotImplementedException();
        }
    }
}
