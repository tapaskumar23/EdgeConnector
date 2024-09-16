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
        public static bool IsValid(this MessageListenerConfiguration? configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            return configuration.Port > 0 && configuration.Port <= 65535;
        }

        public static bool IsValid(this HttpEndpointConfiguration? configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            return configuration.FullyQualifiedEndpoint != null && configuration.FullyQualifiedEndpoint.IsAbsoluteUri;
        }

        public static bool IsValid(this MqttEndpointConfiguration? configuration)
        {
            try
            {
                configuration?.ToMqttConnectionSettings();
                return true;
            }
            catch (Exception e)
            {
                return e == null;
            }
        }

        public static bool IsValid(this LocalStorageEndpointConfiguration? configuration)
        {
            return !string.IsNullOrWhiteSpace(configuration?.Path);
        }

        public static bool IsEmpty(this MqttEndpointConfiguration? configuration)
        {
            if (configuration == null)
            {
                return true;
            }

            int count = configuration.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Count(prop => !string.IsNullOrWhiteSpace(GetStringValue(configuration, prop)));

            return count == 0;
        }

        public static MqttConnectionSettings ToMqttConnectionSettings(this MqttEndpointConfiguration? configuration)
        {
            if (configuration != null)
            {
                IEnumerable<string> settings = configuration.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => !string.IsNullOrWhiteSpace(GetStringValue(configuration, prop)))
                    .Select(prop => $"{prop.Name}={GetStringValue(configuration, prop)}");

                string connectionString = string.Join(";", settings);

                return MqttConnectionSettings.FromConnectionString(connectionString);
            }

            throw new InvalidConfigurationException($"{nameof(MqttEndpointConfiguration)} is invalid.");
        }

        private static string GetStringValue(MqttEndpointConfiguration configuration, PropertyInfo propertyInfo)
        {
            object? val = propertyInfo.GetValue(configuration);

            if (val != null && val is string outVal && !string.IsNullOrWhiteSpace(outVal))
            {
                return outVal;
            }

            return string.Empty;
        }
    }
}
