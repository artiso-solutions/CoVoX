using System;
using Microsoft.Extensions.Configuration;

namespace API
{
    public class SecretsHelper
    {
        public static AzureConfiguration GetSecrets()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("secrets.json")
                .Build();

            var section = config.GetSection(nameof(AzureConfiguration));

            return section.Get<AzureConfiguration>();
        }
    }
}
