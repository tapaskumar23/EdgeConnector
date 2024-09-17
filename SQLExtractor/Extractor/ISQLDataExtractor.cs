using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public interface ISQLDataExtractor<TSQLDataContext>
        where TSQLDataContext : SQLDataContext
    {
        event Func<TSQLDataContext, Task> OnDataExtracted;
        Task ExtractData(CancellationToken cancellationToken);

        Task<bool>  VerifySQLConnection(CancellationToken cancellationToken);

        Task PerformInitalSetup(CancellationToken cancellationToken);

    }
}
