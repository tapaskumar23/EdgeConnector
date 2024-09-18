// --------------------------------------------------------------------------
// <copyright file="IMessageContextFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using FellowOakDicom.Network;

namespace Microsoft.Health.DICOM.Listener.Files

{
    public interface IDICOMFileContextFactory
        <TDICOMFileContext>
        where TDICOMFileContext : DICOMFileContext
    {
        TDICOMFileContext Create(byte[] file);
    }
}
