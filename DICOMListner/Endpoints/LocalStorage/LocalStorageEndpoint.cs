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
using FellowOakDicom;
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
                // Load the DICOM file into memory using fo-dicom
                DicomFile dicomFile;
                using (var memoryStream = new MemoryStream(file))
                {
                    dicomFile = DicomFile.Open(memoryStream, FileReadOption.Default);
                }

                // Extract the modality from the DICOM file
                string modality = dicomFile.Dataset.GetSingleValueOrDefault(DicomTag.Modality, "Unknown");

                // Creating a file path based on the extracted modality and using .dcm extension
                string fileName = context.EnqueueDateTimeOffset.ToString("yyyy-MM-ddTHH-mm-ss-ffffff", CultureInfo.InvariantCulture) + ".dcm";
                string modalityFolder = Path.Combine("Bronz", "DICOM", modality);
                Directory.CreateDirectory(modalityFolder); // Ensure the folder exists

                string filePath = Path.Combine(modalityFolder, fileName);

                // Writing the file to the designated path
                using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                fileStream.WriteAsync(file, 0, file.Length, cancellationToken);

                _logger.LogInformation("DICOM file written to local storage successfully at: {FilePath}", filePath);
                return Task.FromResult<(string, string)?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write DICOM file to local storage.");
                return Task.FromResult<(string, string)?>((ex.GetType().Name, ex.Message));
            }
        }

        //public Task<(string code, string error)?> Send(LocalStorageDICOMFileContext context, Byte[] file, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        // Creating a file path with .dcm extension
        //        string fileName = context.EnqueueDateTimeOffset.ToString("yyyy-MM-ddTHH-mm-ss-ffffff", CultureInfo.InvariantCulture) + ".dcm";
        //        string filePath = Path.Combine(_path, fileName);

        //        // Creating a FileStream and StreamWriter to write the file
        //        using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);

        //        // Write the byte array directly to the file stream (no need for StreamWriter with binary files)
        //        fileStream.Write(file, 0, file.Length);

        //        _logger.LogInformation("DICOM file written to local storage successfully: {FilePath}", filePath);
        //        return Task.FromResult<(string, string)?>(null);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to write DICOM file to local storage.");
        //        return Task.FromResult<(string, string)?>((ex.GetType().Name, ex.Message));
        //    }
        //}

    }
}
