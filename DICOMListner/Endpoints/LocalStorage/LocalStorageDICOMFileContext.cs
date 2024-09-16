// --------------------------------------------------------------------------
// <copyright file="LocalStorageMessageContext.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Microsoft.Health.DICOM.Listener.Files;

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public class LocalStorageDICOMFileContext : DICOMFileContext
    {
        public LocalStorageDICOMFileContext(DateTimeOffset enqueueDateTimeOffset)
            : base(enqueueDateTimeOffset)
        {
        }
    }
}
