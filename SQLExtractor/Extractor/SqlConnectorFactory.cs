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
            ArgumentNullException.ThrowIfNullOrEmpty(server);
            ArgumentNullException.ThrowIfNullOrEmpty(database);
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            ArgumentNullException.ThrowIfNullOrEmpty(password);

            string connectionString = $"Server={server};Database={database};User Id={username};Password={password};";
            //string conn = @"Server=NU-VGULAGANNAVA\SQLEXPRESS,32000;Database=AdventureWorksDW2019;User Id=testUser;Password=Vinod!YTAasK61#44;";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
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
