// --------------------------------------------------------------------------
// <copyright file="IMessageContextFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------


using FellowOakDicom.Network;

namespace Microsoft.Health.DICOM.Listener.Messages
{
    public interface IMessageContextFactory<TMessageContext>
        where TMessageContext : MessageContext
    {
        TMessageContext Create(DicomMessage message);
    }
}
