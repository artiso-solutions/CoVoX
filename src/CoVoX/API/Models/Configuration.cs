using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Covox
{
    public class Configuration
    {
        [Required]
        public AzureConfiguration AzureConfiguration { get; set; }

        [Required]
        [Range(0, 1)]
        public double MatchingThreshold { get; set; } = 0.95;

        [Required]
        public IReadOnlyList<string> InputLanguages { get; set; }

        public IReadOnlyList<string> HotWords { get; set; }
    }

    public class AzureConfiguration
    {
        [Required]
        public string SubscriptionKey { get; set; }

        [Required]
        public string Region { get; set; }

        public static AzureConfiguration FromSubscription(string subscriptionKey, string region)
        {
            return new AzureConfiguration
            {
                SubscriptionKey = subscriptionKey,
                Region = region
            };
        }

        public static AzureConfiguration FromFile(string secretsPath = "secrets.json")
        {
            return SecretsHelper.GetAzureConfigurationFromSecretsJson(secretsPath);
        }

        public static AzureConfiguration FromEnvironmentVariable(string variableName = "COVOX_AZURE_CONFIG")
        {
            return SecretsHelper.GetAzureConfigurationFromEnvironmentVariables(variableName);
        }
    }
}
