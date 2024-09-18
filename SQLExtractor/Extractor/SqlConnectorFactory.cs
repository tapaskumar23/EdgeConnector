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
            return new DataTable();
        }

        public bool TestConnection(string server, string database, string username, string password)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(server);
            ArgumentNullException.ThrowIfNullOrEmpty(database);
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            ArgumentNullException.ThrowIfNullOrEmpty(password);

            //string conn = $"Server={server};Database={database};User Id={username};Password={password};";
            //172.24.192.1
            string conn = @"Server=192.168.1.3,1433;Database=AdventureWorksDW2019;User Id=testUser;Password=Vinod!YTAasK61#44;";
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                try
                {
                    sqlConnection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    string exceptionDetail = ex.Message;
                    return false;
                }
            }
        }
    }
}
