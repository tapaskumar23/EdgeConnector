using EnsureThat;
using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage
{
    public class LocalStorageSQLDataContextFactory : ISQLDataContextFactory<LocalStorageSQLDataContext>
    {
        private readonly TimeProvider _timeProvider;

        public LocalStorageSQLDataContextFactory(TimeProvider timeProvider)
        {
            _timeProvider = EnsureArg.IsNotNull(timeProvider, nameof(timeProvider));
        }
        public LocalStorageSQLDataContext Create(DateTimeOffset enqueueDateTimeOffset)
        {
            return new LocalStorageSQLDataContext(enqueueDateTimeOffset);
        }
    }
}
