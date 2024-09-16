// --------------------------------------------------------------------------
// <copyright file="ILocalStorageEndpoint.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public interface ILocalStorageEndpoint : IExternalEndpoint<LocalStorageMessageContext>
    {
    }
}
