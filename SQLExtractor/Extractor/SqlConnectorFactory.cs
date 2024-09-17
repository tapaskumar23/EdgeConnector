using Microsoft.Health.SQL.Extractor.Configuration;
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
    public class SqlConnectorFactory : ISqlConnectorFactory
    {
        public SqlConnection Create(string server, string database, string username, string password)
        {
            return new SqlConnection($"Server={server};Database={database};User Id={username};Password={password};");
        }

        public DataTable Execute(SqlConnection connection, string query)
        {
            throw new NotImplementedException();
        }

        public bool TestConnection(string server, string database, string username, string password)
        {
            using (SqlConnection sqlConnection = new SqlConnection($"Server={server};Database={database};User Id={username};Password={password};"))
            {
                try
                {
                    sqlConnection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
