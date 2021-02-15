using System;
using Microsoft.Extensions.Configuration;

namespace CustomCommands.Configuration.AppSettings
{
    internal static class AppSettingsHelper{
        
        internal static CustomCommandClientConfiguration GetSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            
            var section = config.GetSection(nameof(CustomCommandClientConfiguration));
            
            return section.Get<CustomCommandClientConfiguration>();
        }
    }
}
