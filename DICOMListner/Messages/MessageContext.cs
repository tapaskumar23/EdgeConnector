// --------------------------------------------------------------------------
// <copyright file="MessageContext.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Microsoft.Health.DICOM.Listener.Messages
{
    public class MessageContext
    {
        public MessageContext(DateTimeOffset enqueueDateTimeOffset)
        {
            EnqueueDateTimeOffset = enqueueDateTimeOffset;
        }

        public DateTimeOffset EnqueueDateTimeOffset { get; }
    }
}
