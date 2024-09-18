using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using Microsoft.Health.SQL.Extractor.Endpoints;
using Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
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
        private readonly IExternalEndpoint<TSQLDataContext> _externalEndpoint;
        private readonly string sqlConfigPath = Directory.GetCurrentDirectory() + @"/SQLScripts/";
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
            IExternalEndpoint<TSQLDataContext> externalEndpoint,
            ISQLDataContextFactory<TSQLDataContext> sqlDataContextFactory)
        {
            _logger = logger;
            _sqlConnectorFactory = sqlConnectorFactory;
            _sqlDataContextFactory = sqlDataContextFactory;
            _sQLConnectorConfiguration = configuration;
            _externalEndpoint = externalEndpoint;
        }

        public async Task ExtractData(CancellationToken cancellationToken)
        {
            //Read the CSV file load into list of tableconfig
            var listOfSqlTableConfig = LoadSQLConfig(sqlConfigPath + "tableconfig.csv");
            if (listOfSqlTableConfig.Result.Count > 0)
            {
                await ProcessSqlTableConfigRecords(listOfSqlTableConfig.Result, cancellationToken);
            }
            // Process each record from the list (Asyn + Parallel)
        }

        public async Task PerformInitalSetup(CancellationToken cancellationToken)
        {
            if (!File.Exists(sqlConfigPath + "tableconfig.csv"))
            {
                var connectionObj = _sqlConnectorFactory.Create(_sQLConnectorConfiguration.Value.ServerIp,
                _sQLConnectorConfiguration.Value.Database,
                _sQLConnectorConfiguration.Value.Username,
                _sQLConnectorConfiguration.Value.Password);

                string path = Directory.GetCurrentDirectory() + @"/SQLScripts/initaltablelist.txt";

                string sqlQuery = File.ReadAllText(path);

                var dataTable = await _sqlConnectorFactory.ExecuteAsync(connectionObj, sqlQuery);
                await DataTableToCSV(dataTable, sqlConfigPath + "tableconfig.csv");
            }
        }

        public async Task<bool> VerifySQLConnection(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_sqlConnectorFactory.TestConnection(_sQLConnectorConfiguration.Value.ServerIp,
                _sQLConnectorConfiguration.Value.Database,
                _sQLConnectorConfiguration.Value.Username,
                _sQLConnectorConfiguration.Value.Password));
        }

        private async Task DataTableToCSV(DataTable dataTable, string filePath)
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
            await File.WriteAllTextAsync(filePath, csvContent.ToString());
        }

        private string EscapeCSV(string value)
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

        private async Task<List<SQLTableConfig>> LoadSQLConfig(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<SQLTableConfig>();
                return new List<SQLTableConfig>(records);
            }
        }
        private async Task ProcessSqlTableConfigRecords(IList<SQLTableConfig> lstSqlTableConfig, CancellationToken cancellationToken)
        {
            var tasks = lstSqlTableConfig.Select(async record =>
            {
                try
                {
                    string sqlText = record.SelectStmt;
                    if (!string.IsNullOrEmpty(sqlText))
                    {
                        var connectionObj = _sqlConnectorFactory.Create(_sQLConnectorConfiguration.Value.ServerIp,
                                            _sQLConnectorConfiguration.Value.Database,
                                            _sQLConnectorConfiguration.Value.Username,
                                            _sQLConnectorConfiguration.Value.Password);
                        var dataTable = await _sqlConnectorFactory.ExecuteAsync(connectionObj, sqlText);
                        dataTable.TableName = record.TableName;

                        if (_externalEndpoint != null)
                        {
                            await _externalEndpoint.SendAsync(_sqlDataContextFactory.Create(DateTimeOffset.UtcNow), dataTable, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to process record: {record}");
                    // Optionally, you can rethrow the exception or handle it as needed
                }
            });

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "One or more tasks failed.");
                // Handle the aggregate exception if needed
            }
        }
    }
}
