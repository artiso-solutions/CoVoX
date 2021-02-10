﻿using System;
using System.Threading.Tasks;
using KeywordRecognition.Settings;
using Microsoft.Extensions.Configuration;

namespace KeywordRecognition
{
    class Program
    {
        static async Task Main()
        {
            var appSettings = GetAppSettings();
            var speechRecognizerClient = new SpeechRecognizerClient(appSettings.SpeechRecognitionClientConfiguration);
            
            await speechRecognizerClient.StartKeyWordRecognition();
            
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


