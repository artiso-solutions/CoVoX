using System;
using System.Threading.Tasks;
using CustomCommands.Configuration;
using CustomCommands.Configuration.AppSettings;
using CustomCommands.Events;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CustomCommands
{
    class Program
    {
        private static async Task Main()
        {
            var configuration = AppSettingsHelper.GetSettings();
            
            var synthesizer = CreateSynthesizer(configuration);
            
            var keywordRecognizerClient = new SpeechRecognizerClient(synthesizer);
            
            var customEventHandlers = new CustomCommandHandler(configuration, synthesizer);
            
            Console.WriteLine("Start Listening...");
            
            while (true)
            {
                // NeverEnded listening for keyword
                await keywordRecognizerClient.StartRecognitionWithKeywordRecognizer("OK_SPEAKER",
                    "Configuration/Keyword/okSpeakerKwd.table");
                 
                // Delay to avoid auto-listening
                await Task.Delay(50);
                
                // Utterance limited listening for commands
                await customEventHandlers.Client.StartListenForCommands();
            }
        }
        
        private static SpeechSynthesizer CreateSynthesizer(CustomCommandClientConfiguration configuration)
        {
            return new(SpeechConfig.FromSubscription(configuration.SubscriptionKey, configuration.Region));
        }
    }
}
