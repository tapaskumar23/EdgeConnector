// --------------------------------------------------------------------------
// <copyright file="LocalStorageEndpoint.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using System.Globalization;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.DICOM.Listener.Configuration;

namespace Microsoft.Health.DICOM.Listener.Endpoints.LocalStorage
{
    public class LocalStorageEndpoint : ILocalStorageEndpoint
    {
        private readonly string _path;
        private readonly ILogger<LocalStorageEndpoint> _logger;

        public LocalStorageEndpoint(
            IOptions<LocalStorageEndpointConfiguration> configuration,
            ILogger<LocalStorageEndpoint> logger)
        {
            _path = EnsureArg.IsNotNullOrWhiteSpace(configuration?.Value?.Path, nameof(configuration.Value.Path));
            _logger = EnsureArg.IsNotNull(logger, nameof(logger));
        }

        public Task<(string code, string error)?> Send(LocalStorageDICOMFileContext context, Byte[] file, CancellationToken cancellationToken)
        {
            try
            {
                string filePath = Path.Combine(_path, context.EnqueueDateTimeOffset.ToString("yyyy-MM-ddTHH-mm-ss-ffffff", CultureInfo.InvariantCulture));

                // Creating a FileStream and StreamWriter addresses a potential race condition
                // when checking for file existence and writing to the file.
                // Using FileMode.CreateNew will throw an excpetion if the file exists.
                using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                using var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(file);

                _logger.LogInformation("Message written to local storage successfully.");
                return Task.FromResult<(string, string)?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write message to local storage.");
                return Task.FromResult<(string, string)?>((ex.GetType().Name, ex.Message));
            }
        }
    }
}
