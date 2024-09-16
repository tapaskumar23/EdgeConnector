using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage
{
    public class LocalStorageSQLDataContextFactory : ISQLDataContextFactory<LocalStorageSQLDataContext>
    {
        public LocalStorageSQLDataContext Create(DateTimeOffset enqueueDateTimeOffset)
        {
            throw new NotImplementedException();
        }
    }
}
