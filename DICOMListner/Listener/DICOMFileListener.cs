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
using Microsoft.Health.DICOM.Listener.Files;

namespace Microsoft.Health.DICOM.Listener.Listener
{
    public class DICOMFileListener<TDICOMFIleContext> : IDICOMFileListener<TDICOMFIleContext>
        where TDICOMFIleContext : DICOMFileContext
    {
        private readonly ILogger<DICOMFileListener<TDICOMFIleContext>> _logger;
        private readonly ITcpListenerFactory _tcpListenerFactory;
        private readonly DICOMFileListenerConfiguration _configuration;
        private readonly IDICOMFileContextFactory<TDICOMFIleContext> _dicomFileContextFactory;

        public DICOMFileListener(
            IOptions<DICOMFileListenerConfiguration> configuration,
            ITcpListenerFactory tcpListenerFactory,
            IDICOMFileContextFactory<TDICOMFIleContext> dicomFileContextFactory,
            ILogger<DICOMFileListener<TDICOMFIleContext>> logger)
        {
            _configuration = EnsureArg.IsNotNull(configuration?.Value, nameof(configuration));
            EnsureArg.IsGt<int>(configuration.Value.Port, 0, nameof(configuration.Value.Port));
            EnsureArg.IsLte<int>(configuration.Value.Port, 65535, nameof(configuration.Value.Port));

            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
            _tcpListenerFactory = EnsureArg.IsNotNull(tcpListenerFactory, nameof(tcpListenerFactory));
            _dicomFileContextFactory = EnsureArg.IsNotNull(dicomFileContextFactory, nameof(dicomFileContextFactory));
        }

        public event Func<TDICOMFIleContext, byte[], CancellationToken, Task<(string code, string error)?>> FileReceived;

        public Task StartAsync(CancellationToken cancellationToken)
        {
             throw new NotImplementedException();

            //start the dicom implementation
            


        }
    }
}
