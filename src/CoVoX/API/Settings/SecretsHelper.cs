using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Covox
{
    internal class SecretsHelper
    {
        /// <summary>
        /// Reads AzureConfiguration from secrets.json file located in the application folder.
        /// </summary>
        /// <param name="path">Path to the secrets.json file.</param>
        /// <returns>AzureConfiguration with values read from secrets.json located in path.</returns>
        internal static AzureConfiguration GetAzureConfigurationFromSecretsJson(string path)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(path)
                .Build();

            var section = config.GetSection(nameof(AzureConfiguration));

            return section.Get<AzureConfiguration>();
        }

        /// <summary>
        /// Reads AzureConfiguration from COVOX_AZURE_CONFIG environment variable. Variable format must be subscriptionKey:region.
        /// </summary>
        /// <param name="variable">Name of the environment variable where the keys are stored.</param>
        /// <returns>AzureConfiguration with values read from environment variable.</returns>
        internal static AzureConfiguration GetAzureConfigurationFromEnvironmentVariables(string variable)
        {
            var config = Environment.GetEnvironmentVariable(variable);

            if (!string.IsNullOrEmpty(config) && config.Contains(':'))
            {
                return new AzureConfiguration
                {
                    SubscriptionKey = config.Split(':').FirstOrDefault(),
                    Region = config.Split(':').LastOrDefault()
                };
            }

            return null;
        }
    }
}
