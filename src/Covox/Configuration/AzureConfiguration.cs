using System;
using System.Linq;
using Covox.Validation;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Covox
{
    public class AzureConfiguration
    {
        private AzureConfiguration() { }

        [NotEmpty]
        public string SubscriptionKey { get; private init; }

        [NotEmpty]
        public string Region { get; private init; }

        /// <summary>
        /// Creates AzureConfiguration from parameters.
        /// </summary>
        /// <param name="subscriptionKey">Azure Cognitive Services Speech subscription key.</param>
        /// <param name="region">Azure Cognitive Services Speech region.</param>
        public static AzureConfiguration FromSubscription(string subscriptionKey, string region)
        {
            return new AzureConfiguration
            {
                SubscriptionKey = subscriptionKey,
                Region = region
            };
        }

        /// <summary>
        /// Reads AzureConfiguration from secrets.json file.
        /// </summary>
        /// <param name="secretsPath">Full path off the secrets.json file.</param>
        /// <returns>AzureConfiguration with values read from secrets.json located in path.</returns>
        public static AzureConfiguration FromFile(string secretsPath = "secrets.json")
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(secretsPath)
                .Build();

            var section = config.GetSection(nameof(AzureConfiguration));

            var children = section.GetChildren().ToArray();

            var subscriptionKey = children.FirstOrDefault(x => x.Key == "SubscriptionKey")?.Value;
            var region = children.FirstOrDefault(x => x.Key == "Region")?.Value;

            return new AzureConfiguration
            {
                SubscriptionKey = subscriptionKey,
                Region = region
            };
        }

        /// <summary>
        /// Reads AzureConfiguration from environment variables.
        /// </summary>
        /// <param name="subscriptionKeyVariable">Environment variable name for Azure subscription key</param>
        /// <param name="regionVariable">Environment variable name for Azure region</param>
        /// <returns>AzureConfiguration with values read from environment variables.</returns>
        public static AzureConfiguration FromEnvironmentVariables(
            string subscriptionKeyVariable = "COVOX_AZURE_SUBSCRIPTION_KEY",
            string regionVariable = "COVOX_AZURE_REGION")
        {
            var subscriptionKey = TryGetEnvironmentVariable(subscriptionKeyVariable);
            var region = TryGetEnvironmentVariable(regionVariable);

            return new AzureConfiguration
            {
                SubscriptionKey = subscriptionKey,
                Region = region
            };

            // Local functions.

            static string TryGetEnvironmentVariable(string varName)
            {
                return
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process) ??
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User) ??
                    Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
            }
        }
    }
}
