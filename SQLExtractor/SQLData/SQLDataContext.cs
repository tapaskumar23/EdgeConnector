using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.SQLData
{
    public class SQLDataContext
    {
        public SQLDataContext(DateTimeOffset enqueueDateTimeOffset)
        {
            EnqueueDateTimeOffset = enqueueDateTimeOffset;
        }

        public DateTimeOffset EnqueueDateTimeOffset { get; }

        public DataTable DataTable { get; set; }
    }
}
