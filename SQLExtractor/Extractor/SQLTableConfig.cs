using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    internal class SQLTableConfig
    {
        public string SelectStmt { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
    }
}
