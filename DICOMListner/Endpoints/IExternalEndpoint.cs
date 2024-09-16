﻿// --------------------------------------------------------------------------
// <copyright file="IExternalEndpoint.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener.Endpoints
{
    public interface IExternalEndpoint<TMessageContext>
        where TMessageContext : MessageContext
    {
        /// <summary>
        /// Sends a message to the external endpoint.
        /// </summary>
        /// <param name="context">Optional context that can be included with the message.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<(string code, string error)?> Send(TMessageContext context, string message, CancellationToken cancellationToken);
    }
}
