using Microsoft.Health.SQL.Extractor.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.SQL.Extractor.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsValid(this SQLConnectorConfiguration? configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(configuration.Username) &&
                !string.IsNullOrEmpty(configuration.Server) &&
                !string.IsNullOrEmpty(configuration.Database) &&
                !string.IsNullOrEmpty(configuration.Password);
        }

        public static bool IsValid(this LocalStorageEndpointConfiguration? configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(configuration.Path) ;
        }
    }
}
