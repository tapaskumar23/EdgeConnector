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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
          await  InitailSetup();

            TcpListener server = _tcpListenerFactory.Create(_configuration.Port);
            server.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting for a connection...");
                TcpClient client = await server.AcceptTcpClientAsync(cancellationToken);
                _logger.LogInformation("Connected!");

                _ = Task.Run(
                    async () =>
                    {
                        try
                        {
                            (byte[] files, NetworkStream stream) = await ReceiveFile(client, cancellationToken);

                            if (FileReceived != null)
                            {

                                (string code, string error)? response = await FileReceived(_dicomFileContextFactory.Create(files),files, cancellationToken);
                                //send ack TBD
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message");
                        }
                        finally
                        {
                            client?.Close();
                        }
                    }, cancellationToken);
            }
        }

        private async Task<(byte[] file, NetworkStream stream)> ReceiveFile(TcpClient client, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receiving file...");

            NetworkStream stream = client.GetStream();
            using var memoryStream = new MemoryStream();
            byte[] buffer = new byte[8192]; // Larger buffer size for better performance

            try
            {
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    _logger.LogInformation("Received {BytesRead} bytes", bytesRead);
                    await memoryStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                }

                _logger.LogInformation("File reception completed.");
                return (memoryStream.ToArray(), stream); // Return the full file data as byte array
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while receiving the file.");
                throw;
            }
        }

        private async Task InitailSetup()
        {
            if (!Directory.Exists(_configuration.FolderStructure))
            {
                Directory.CreateDirectory(_configuration.FolderStructure);
            }
        }
    }
}
