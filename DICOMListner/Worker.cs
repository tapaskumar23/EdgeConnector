// --------------------------------------------------------------------------
// <copyright file="Worker.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Health.DICOM.Listener.Endpoints;
using Microsoft.Health.DICOM.Listener.Listener;
using Microsoft.Health.DICOM.Listener.Messages;

namespace Microsoft.Health.DICOM.Listener
{
    public sealed class Worker<TMessageContext> : BackgroundService
        where TMessageContext : MessageContext
    {
        private readonly IMessageListener<TMessageContext> _DICOMServer;
        private readonly IExternalEndpoint<TMessageContext> _externalEndpoint;
        private readonly ILogger<Worker<TMessageContext>> _logger;

        public Worker(
            IMessageListener<TMessageContext> DICOMServer,
            IExternalEndpoint<TMessageContext> externalEndpointClient,
            ILogger<Worker<TMessageContext>> logger)
        {
            _DICOMServer = EnsureArg.IsNotNull(DICOMServer, nameof(DICOMServer));
            _externalEndpoint = EnsureArg.IsNotNull(externalEndpointClient, nameof(externalEndpointClient));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));

            _DICOMServer.MessageReceived += HandleMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting DICOM listener");
            await _DICOMServer.StartAsync(stoppingToken);
        }

        private async Task<(string code, string error)?> HandleMessage(TMessageContext context, string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending message to external endpoint.");
            return await _externalEndpoint.Send(context, message, cancellationToken);
        }

        public override void Dispose()
        {
            _DICOMServer.MessageReceived -= HandleMessage;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
