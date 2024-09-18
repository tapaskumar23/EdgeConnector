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

        public async Task<DataTable> ExecuteAsync(SqlConnection connection, string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                await connection.OpenAsync();
                Console.WriteLine("Connection Opened Successfully");

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                await connection.CloseAsync();
            }

            return dataTable;
        }

        public bool TestConnection(string serverIP, string database, string username, string password)
        {          
            string conn = $"Server={serverIP},1433;Database={database};User Id={username};Password={password};";
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
