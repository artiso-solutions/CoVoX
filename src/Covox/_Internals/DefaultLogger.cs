using Microsoft.Extensions.Logging;

namespace Covox
{
    internal static class DefaultLogger
    {
        public static ILogger<T> CreateLogger<T>(LogLevel logLevel = LogLevel.Trace)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddDebug()
                .SetMinimumLevel(logLevel));

            return loggerFactory.CreateLogger<T>();
        }
    }
}
