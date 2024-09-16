using Microsoft.Health.DICOM.Listener.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.DICOM.Listener.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsValid(this DICOMFileListenerConfiguration? configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            return configuration.Port > 0 && configuration.Port <= 65535;
        }

        public static bool IsValid(this LocalStorageEndpointConfiguration? configuration)
        {
            return !string.IsNullOrWhiteSpace(configuration?.Path);
        }

    }
}
