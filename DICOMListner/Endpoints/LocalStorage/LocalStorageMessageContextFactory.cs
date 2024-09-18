// --------------------------------------------------------------------------
// <copyright file="LocalStorageMessageContextFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using EnsureThat;
using FellowOakDicom;
using FellowOakDicom.Network;
using Microsoft.Health.DICOM.Listener.Files;

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public class LocalStorageMessageContextFactory : IDICOMFileContextFactory<LocalStorageDICOMFileContext>
    {
        private readonly TimeProvider _timeProvider;

        public LocalStorageMessageContextFactory(TimeProvider timeProvider)
        {
            _timeProvider = EnsureArg.IsNotNull(timeProvider, nameof(timeProvider));
        }

        public LocalStorageDICOMFileContext Create(byte[] file)
        {
            return new LocalStorageDICOMFileContext(_timeProvider.GetUtcNow());
        }

    }
}
