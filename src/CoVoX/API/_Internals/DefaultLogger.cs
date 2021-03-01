using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Covox
{
    internal static class DefaultLogger
    {
        public static ILogger<T> CreateLogger<T>(LogLevel logLevel = LogLevel.Trace)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.ColorBehavior = LoggerColorBehavior.Disabled;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy-MM-yy HH:mm:ss.fff zzz ";
                }).SetMinimumLevel(logLevel));

            return loggerFactory.CreateLogger<T>();
        }
    }
}
