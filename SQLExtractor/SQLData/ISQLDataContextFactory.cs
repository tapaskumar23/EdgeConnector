using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.SQLData
{
    public interface ISQLDataContextFactory<TSQLDataContext> where TSQLDataContext : SQLDataContext
    {
        TSQLDataContext Create(DateTimeOffset enqueueDateTimeOffset);
    }
}
