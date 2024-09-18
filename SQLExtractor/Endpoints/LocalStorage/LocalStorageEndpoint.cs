using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.SQL.Extractor.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage
{
    public class LocalStorageEndpoint : ILocalStorageEndpoint
    {
        private readonly string _path;
        private readonly string _serverName;
        private readonly string _dbName;
        private readonly ILogger<LocalStorageEndpoint> _logger;

        public LocalStorageEndpoint(
            IOptions<LocalStorageEndpointConfiguration> configuration,
            IOptions<SQLConnectorConfiguration> sqlConfiguration,
            ILogger<LocalStorageEndpoint> logger)
        {
            _path = EnsureArg.IsNotNullOrWhiteSpace(configuration?.Value?.Path, nameof(configuration.Value.Path));
            _serverName = EnsureArg.IsNotNullOrWhiteSpace(sqlConfiguration?.Value?.ServerName, nameof(configuration.Value.Path));
            _dbName = EnsureArg.IsNotNullOrWhiteSpace(sqlConfiguration?.Value?.Database, nameof(configuration.Value.Path));


            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }

        public Task PerformInitialSetupAsync(string serverName, string databaseName)
        {
            string serverPath = Path.Combine(_path, serverName);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            string databaseNamePath = Path.Combine(_path, serverName, databaseName);
            if (!Directory.Exists(databaseNamePath))
            {
                Directory.CreateDirectory(databaseNamePath);
            }

            return Task.CompletedTask;
        }

        public async Task<(string code, string error)?> SendAsync(LocalStorageSQLDataContext context, DataTable sqlData, CancellationToken cancellationToken)
        {
            try
            {
                string filePath = Path.Combine(_path, _serverName, _dbName, sqlData.TableName, context.EnqueueDateTimeOffset.ToString("yyyy-MM-ddTHH-mm-ss-ffffff", CultureInfo.InvariantCulture) + ".csv");

                string dirfilePath = Path.Combine(_path, _serverName, _dbName, sqlData.TableName);

                if (!Directory.Exists(dirfilePath))
                    Directory.CreateDirectory(dirfilePath);

                await DataTableToCSV(sqlData, filePath);
                _logger.LogInformation("Message written to local storage successfully.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write message to local storage.");
                return (ex.GetType().Name, ex.Message);
            }
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
    }
}
