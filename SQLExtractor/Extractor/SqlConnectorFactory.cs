using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public class SqlConnectorFactory : ISqlConnectorFactory<SQLDataContext>
    {
        public SqlConnection Create(string server, string database, string username, string password)
        {
            return new SqlConnection($"Server={server};Database={database};User Id={username};Password={password};");
        }
    }
}
