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
            
            var customEventHandlers = new CustomBaseEventHandlers(synthesizer);
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            
            var customCommands = new CustomCommandsClient(configuration, audioConfig, customEventHandlers);
                        
            var keywordRecognizerClient = new SpeechRecognizerClient(synthesizer);

            while (true)
            {
                // NeverEnded listening for keyword
                await keywordRecognizerClient.StartRecognitionWithKeywordRecognizer("OK_SPEAKER",
                    "Configuration/Keyword/okSpeakerKwd.table");
                
                // Utterance limited listening for commands
                await customCommands.StartListenForCommands();
            }
        }
        
        private static SpeechSynthesizer CreateSynthesizer(CustomCommandClientConfiguration configuration)
        {
            return new(SpeechConfig.FromSubscription(configuration.SubscriptionKey, configuration.Region));
        }
    }
}
