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
            // throw new NotImplementedException();

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
                            (List<byte[]> files, NetworkStream stream) = await ReceiveFrame(client, cancellationToken);

                            if (FileReceived == null && files.Count > 0)
                            {
                                foreach (var file in files)
                                {
                                    (string code, string error)? response = await FileReceived(_dicomFileContextFactory.Create(file),file, cancellationToken);
                                    // await SendAckMessage(stream, message, "response?.code", "response?.error", cancellationToken);
                                }
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

        private async Task<(List<byte[]> files, NetworkStream stream)> ReceiveFrame(TcpClient client, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receiving file frame...");

            NetworkStream stream = client.GetStream();
            var fileChunks = new List<byte[]>(); // List to store file chunks
            byte[] buffer = new byte[8192]; // Increased buffer size for better performance

            while (!cancellationToken.IsCancellationRequested)
            {
                if (stream.DataAvailable)
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                        if (bytesRead > 0)
                        {
                            _logger.LogInformation("Received {BytesRead} bytes", bytesRead);

                            // Store the chunk of data read
                            byte[] chunk = new byte[bytesRead];
                            Buffer.BlockCopy(buffer, 0, chunk, 0, bytesRead);
                            fileChunks.Add(chunk);
                        }
                    }
                    while (stream.DataAvailable);

                    // Return the collected file chunks and the stream
                    return (fileChunks, stream);
                }
                else
                {
                    await Task.Yield(); // Yield control without busy waiting
                }
            }

            throw new OperationCanceledException();
        }


        //private async Task<(byte[] fileData, NetworkStream stream)> ReceiveFrame(TcpClient client, CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Receiving file frame...");

        //    NetworkStream stream = client.GetStream();
        //    using (var memoryStream = new MemoryStream()) // MemoryStream to store the entire file data
        //    {
        //        byte[] buffer = new byte[8192]; // Increased buffer size for better performance

        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            if (stream.DataAvailable)
        //            {
        //                int bytesRead;
        //                do
        //                {
        //                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

        //                    if (bytesRead > 0)
        //                    {
        //                        _logger.LogInformation("Received {BytesRead} bytes", bytesRead);

        //                        // Write the received data to the MemoryStream
        //                        await memoryStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
        //                    }
        //                }
        //                while (stream.DataAvailable);

        //                // Return the file data as a byte array along with the stream
        //                return (memoryStream.ToArray(), stream);
        //            }
        //            else
        //            {
        //                await Task.Yield(); // Yield control without busy waiting
        //            }
        //        }
        //    }

        //    throw new OperationCanceledException();
        //}


        //private async Task<(List<byte[]> files, NetworkStream stream)> ReceiveFrame(TcpClient client, CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Receiving file frame...");

        //    NetworkStream stream = client.GetStream();
        //    var frame = new StringBuilder();
        //    byte[] buffer = new byte[256];

        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        if (stream.DataAvailable)
        //        {
        //            int bytesRead;
        //            do
        //            {
        //                bytesRead = await stream.ReadAsync(buffer, cancellationToken);

        //                if (bytesRead > 0)
        //                {
        //                    _logger.LogInformation("Received {BytesRead} bytes", bytesRead);

        //                    for (int i = 0; i < bytesRead; i++)
        //                    {
        //                        frame.Append((char)buffer[i]);
        //                    }
        //                }
        //            }
        //            while (stream.DataAvailable);

        //            return (ProcessFrame(frame.ToString()), stream);
        //        }
        //        else
        //        {
        //            await Task.Delay(100, cancellationToken); // Avoid busy waiting
        //        }
        //    }

            //    throw new OperationCanceledException();
            //}

            private List<byte[]> ProcessFrame(string frame)
        {
            _logger.LogInformation("Processing frame: {Frame}", frame);
            // process message if needed

            // convert frame to byte array
            return new List<byte[]> { Encoding.UTF8.GetBytes(frame) };


        }

    }
}
