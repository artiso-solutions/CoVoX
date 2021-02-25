using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Covox
{
    public class SecretsHelper
    {
        /// <summary>
        /// Reads AzureConfiguration from secrets.json file located in the application folder.
        /// </summary>
        /// <returns>AzureConfiguration with values read from secrets.json.</returns>
        public static AzureConfiguration GetAzureConfigurationFromSecretsJson()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("secrets.json")
                .Build();

            var section = config.GetSection(nameof(AzureConfiguration));

            return section.Get<AzureConfiguration>();
        }

        /// <summary>
        /// Reads AzureConfiguration from COVOX_AZURE_CONFIG environment variable. Variable format must be subscriptionKey:region.
        /// </summary>
        /// <returns>AzureConfiguration with values read from environment variable.</returns>
        public static AzureConfiguration GetAzureConfigurationFromEnvironmentVariables()
        {
            var config = Environment.GetEnvironmentVariable("COVOX_AZURE_CONFIG");

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
