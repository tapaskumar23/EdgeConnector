// --------------------------------------------------------------------------
// <copyright file="TcpListenerFactory.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using System.Net;
using System.Net.Sockets;

namespace Microsoft.Health.DICOM.Listener.Listener
{
    public class TcpListenerFactory : ITcpListenerFactory
    {
        public TcpListener Create(int port)
        {
            return new TcpListener(IPAddress.Any, port);
        }
    }
}
