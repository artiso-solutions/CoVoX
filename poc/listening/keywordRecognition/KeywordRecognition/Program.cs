using System;
using KeywordRecognition.Settings;
using Microsoft.Extensions.Configuration;

namespace KeywordRecognition
{
    class Program
    {
        static void Main()
        {
            var appSettings = GetAppSettings();
            var speechRecognizerClient = new SpeechRecognizerClient(appSettings.SpeechRecognitionClientConfiguration);
            
            _ = speechRecognizerClient.StartRecognitionWithKeywordRecognizer("HEY_COVOX", "SpeechStudio/heyCovox.table")
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                        throw task.Exception;
                    
                });
            _ = speechRecognizerClient.StartRecognitionWithKeywordRecognizer("HELLO_COMPUTER", "SpeechStudio/helloComputer.table")
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                        throw task.Exception;
                    
                });
            
            Console.ReadLine();
        }

        #region GetAppSettings

        private static AppSettings GetAppSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var section = config.GetSection(nameof(AppSettings));

            return section.Get<AppSettings>();
        }

        #endregion
    }
}
