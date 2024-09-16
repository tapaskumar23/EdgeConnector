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
using Microsoft.Health.DICOM.Listener.Files;

namespace Microsoft.Health.DICOM.Listener
{
    public sealed class Worker<TDICOMFileContext> : BackgroundService
        where TDICOMFileContext : DICOMFileContext
    {
        private readonly IDICOMFileListener
            <TDICOMFileContext> _DICOMServer;
        private readonly IExternalEndpoint<TDICOMFileContext> _externalEndpoint;
        private readonly ILogger<Worker<TDICOMFileContext>> _logger;

        public Worker(
            IDICOMFileListener
            <TDICOMFileContext> DICOMServer,
            IExternalEndpoint<TDICOMFileContext> externalEndpointClient,
            ILogger<Worker<TDICOMFileContext>> logger)
        {
            _DICOMServer = EnsureArg.IsNotNull(DICOMServer, nameof(DICOMServer));
            _externalEndpoint = EnsureArg.IsNotNull(externalEndpointClient, nameof(externalEndpointClient));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));

            _DICOMServer.FileReceived += HandleMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting DICOM listener");
            await _DICOMServer.StartAsync(stoppingToken);
        }

        private async Task<(string code, string error)?> HandleMessage(TDICOMFileContext context, byte[] data, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending message to external endpoint.");
            return await _externalEndpoint.Send(context, data, cancellationToken);
        }

        public override void Dispose()
        {
            _DICOMServer.FileReceived -= HandleMessage;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
