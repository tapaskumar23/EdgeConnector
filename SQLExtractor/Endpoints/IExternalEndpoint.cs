// --------------------------------------------------------------------------
// <copyright file="IExternalEndpoint.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------


using Microsoft.Health.SQL.Extractor.SQLData;
using System.Data;

namespace Microsoft.Health.SQL.Extractor.Endpoints
{
    public interface IExternalEndpoint<TSQLDataContext>
        where TSQLDataContext : SQLDataContext
    {/// <summary>
     /// Send data to the external endpoint
     /// </summary>
     /// <param name="context"></param>
     /// <param name="sqlData"></param>
     /// <param name="cancellationToken"></param>
     /// <returns></returns>
        Task<(string code, string error)?> Send(TSQLDataContext context, DataTable sqlData, CancellationToken cancellationToken);
    }
}
