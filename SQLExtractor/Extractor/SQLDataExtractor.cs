using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<DataTable> ExtractData(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PerformInitalSetup(CancellationToken cancellationToken)
        {
            if (VerifySQLConnection(cancellationToken).Result)
            {
                var connectionObj = _sqlConnectorFactory.Create(_sQLConnectorConfiguration.Value.Server,
                _sQLConnectorConfiguration.Value.Database,
                _sQLConnectorConfiguration.Value.Username,
                _sQLConnectorConfiguration.Value.Password);

                string path = Directory.GetCurrentDirectory() + @"SQLScripts/initaltablelist.txt";

                string sqlQuery = File.ReadAllText(path);

                var dataTable = _sqlConnectorFactory.Execute(connectionObj, sqlQuery);
                
            }
            return Task.CompletedTask;
        }

        public async Task<bool> VerifySQLConnection(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_sqlConnectorFactory.TestConnection(_sQLConnectorConfiguration.Value.Server,
                _sQLConnectorConfiguration.Value.Database,
                _sQLConnectorConfiguration.Value.Username,
                _sQLConnectorConfiguration.Value.Password));
        }

        public void DataTableToCSV(DataTable dataTable, string filePath)
        {
            StringBuilder csvContent = new StringBuilder();

            // Add column names
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (i > 0)
                    csvContent.Append(",");
                csvContent.Append(EscapeCSV(dataTable.Columns[i].ColumnName));
            }
            csvContent.AppendLine();

            // Add rows
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i > 0)
                        csvContent.Append(",");
                    csvContent.Append(EscapeCSV(row[i].ToString()));
                }
                csvContent.AppendLine();
            }

            // Write to file
            File.WriteAllText(filePath, csvContent.ToString());
        }

        public string EscapeCSV(string value)
        {
            // Escape quotes and wrap with double quotes if needed
            if (value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
            }
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = "\"" + value + "\"";
            }
            return value;
        }
    }
}
