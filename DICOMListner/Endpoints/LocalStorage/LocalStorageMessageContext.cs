// --------------------------------------------------------------------------
// <copyright file="LocalStorageMessageContext.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public class LocalStorageMessageContext : MessageContext
    {
        public LocalStorageMessageContext(DateTimeOffset enqueueDateTimeOffset)
            : base(enqueueDateTimeOffset)
        {
        }
    }
}
