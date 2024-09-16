// --------------------------------------------------------------------------
// <copyright file="MessageListener.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using System.Net.Sockets;
using System.Text;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.DICOM.Listener.Configuration;
using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener.Listener
{
    public class MessageListener<TMessageContext> : IMessageListener<TMessageContext>
        where TMessageContext : MessageContext
    {
        private readonly ILogger<MessageListener<TMessageContext>> _logger;
        private readonly ITcpListenerFactory _tcpListenerFactory;
        private readonly MessageListenerConfiguration _configuration;
        private readonly IMessageContextFactory<TMessageContext> _messageContextFactory;

        public MessageListener(
            IOptions<MessageListenerConfiguration> configuration,
            ITcpListenerFactory tcpListenerFactory,
            IMessageContextFactory<TMessageContext> messageContextFactory,
            ILogger<MessageListener<TMessageContext>> logger)
        {
            _configuration = EnsureArg.IsNotNull(configuration?.Value, nameof(configuration));
            EnsureArg.IsGt<int>(configuration.Value.Port, 0, nameof(configuration.Value.Port));
            EnsureArg.IsLte<int>(configuration.Value.Port, 65535, nameof(configuration.Value.Port));

            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _tcpListenerFactory = EnsureArg.IsNotNull(tcpListenerFactory, nameof(tcpListenerFactory));
            _messageContextFactory = EnsureArg.IsNotNull(messageContextFactory, nameof(messageContextFactory));
        }

        public event Func<TMessageContext, string, CancellationToken, Task<(string code, string error)?>> MessageReceived;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
