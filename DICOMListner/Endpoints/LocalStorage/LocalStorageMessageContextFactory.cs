// --------------------------------------------------------------------------
// <copyright file="LocalStorageMessageContextFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using EnsureThat;
using FellowOakDicom;
using FellowOakDicom.Network;
using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public class LocalStorageMessageContextFactory : IMessageContextFactory<LocalStorageMessageContext>
    {
        private readonly TimeProvider _timeProvider;

        public LocalStorageMessageContextFactory(TimeProvider timeProvider)
        {
            _timeProvider = EnsureArg.IsNotNull(timeProvider, nameof(timeProvider));
        }

        public LocalStorageMessageContext Create(DicomMessage message)
        {
            return new LocalStorageMessageContext(_timeProvider.GetUtcNow());
        }

    }
}
