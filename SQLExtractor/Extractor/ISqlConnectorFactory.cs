using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public interface ISqlConnectorFactory
    {
        SqlConnection Create(string server, string database, string username, string password);

        bool TestConnection(string server, string database, string username, string password);

        Task<DataTable> ExecuteAsync(SqlConnection connection, string query);
    }
}
