using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public interface ISQLDataExtractor<TSQLDataContext> where TSQLDataContext : SQLData.SQLDataContext
    {
        event Func<TSQLDataContext, Task> OnDataExtracted;
        Task ExtractData(CancellationToken cancellationToken);

        Task VerifySQLConnection(CancellationToken cancellationToken);

        Task PerformInitalSetup(CancellationToken cancellationToken);

    }
}
