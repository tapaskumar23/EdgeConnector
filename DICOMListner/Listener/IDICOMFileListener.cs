// --------------------------------------------------------------------------
// <copyright file="IMessageListener.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Microsoft.Health.DICOM.Listener.Files;

namespace Microsoft.Health.DICOM.Listener.Listener
{
    /// <summary>
    /// Interface for a server that listens for DICOM messages.
    /// </summary>
    /// <typeparam name="TDICOMFileContext">The concrete MessageContext type </typeparam>
    public interface IDICOMFileListener<TDICOMFileContext>
        where TDICOMFileContext : DICOMFileContext
    {
        /// <summary>
        /// Event that is triggered when a message is received.
        /// </summary>
        /// <param name="context">Optional context that can be included with the message.</param>
        /// <param name="message">The received message.</param>
        /// <param name="cancellationToken">A CancellationToken that can be used to cancel the operation.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains an optional tuple with the response code and error message.</returns>
        event Func<TDICOMFileContext, byte[], CancellationToken, Task<(string code, string error)?>> FileReceived;

        /// <summary>
        /// Starts the server and begins listening for messages.
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken that can be used to cancel the operation.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken);
    }
}
