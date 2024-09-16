// --------------------------------------------------------------------------
// <copyright file="IExternalEndpoint.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------


using System.Data;

namespace Microsoft.Health.SQL.Extractor.Endpoints
{
    public interface IExternalEndpoint<TDataContext>
        where TDataContext : DataTable
    {/// <summary>
     /// Send data to the external endpoint
     /// </summary>
     /// <param name="context"></param>
     /// <param name="message"></param>
     /// <param name="cancellationToken"></param>
     /// <returns></returns>
        Task<(string code, string error)?> Send(TDataContext context, string message, CancellationToken cancellationToken);
    }
}
