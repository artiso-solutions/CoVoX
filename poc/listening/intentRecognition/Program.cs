using System;
using System.Threading.Tasks;
using IntentRecognition.Settings;
using Microsoft.Extensions.Configuration;

namespace IntentRecognition
{
    internal static class Program
    {
        static async Task Main()
        {
            // IntentRecognition with the IntentRecognizer
            var appSettings = GetAppSettings();
            var languageUnderstandingClient = new LanguageUnderstandingClient(appSettings.LanguageUnderstandingClientConfiguration);
            var intentResponse = await languageUnderstandingClient.GetIntentWithIntentRecognizer();
            Console.WriteLine(intentResponse);
            
            // IntentRecognition with SpeechRecognizer and LanguageUnderstanding RestClient
            // var languageUnderstandingClient1 = new LanguageUnderstandingClient(appSettings.LanguageUnderstandingClientConfiguration);
            // var response1 = await languageUnderstandingClient1.GetIntentWithRestClient(appSettings.SpeechRecognitionClientConfiguration);
            // Console.WriteLine(response1);
            
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
