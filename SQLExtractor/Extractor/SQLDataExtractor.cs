﻿using Microsoft.Health.SQL.Extractor.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extractor
{
    public class SQLDataExtractor : ISQLDataExtractor<SQLDataContext>
    {
        public event Func<SQLDataContext, Task> OnDataExtracted;

        public Task ExtractData(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
