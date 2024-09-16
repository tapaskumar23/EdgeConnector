// --------------------------------------------------------------------------
// <copyright file="MessageContext.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Microsoft.Health.DICOM.Listener.Files
{
    public class DICOMFileContext
    {
        public DICOMFileContext(DateTimeOffset enqueueDateTimeOffset)
        {
            EnqueueDateTimeOffset = enqueueDateTimeOffset;
        }

        public DateTimeOffset EnqueueDateTimeOffset { get; }
    }
}
