using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public interface ISqlConnectorFactory<TSQLDataContext> where TSQLDataContext : SQLDataContext
    {
        SqlConnection Create(string server, string database, string username, string password);
    }
}
