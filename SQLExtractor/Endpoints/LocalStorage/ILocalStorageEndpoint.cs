using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Endpoints.LocalStorage
{
    public interface ILocalStorageEndpoint : IExternalEndpoint<LocalStorageSQLDataContext>
    {
    }
}
