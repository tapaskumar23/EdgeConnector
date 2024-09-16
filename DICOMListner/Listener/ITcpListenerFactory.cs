// --------------------------------------------------------------------------
// <copyright file="ITcpListenerFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using System.Net.Sockets;

namespace Microsoft.Health.DICOM.Listener.Listener
{
    /// <summary>
    /// Interface for a factory that creates TcpListener instances.
    /// </summary>
    public interface ITcpListenerFactory
    {
        /// <summary>
        /// Creates a new TcpListener instance that listens on the specified port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        /// <returns>A TcpListener instance.</returns>
        TcpListener Create(int port);
    }
}
