using System;
using System.Threading.Tasks;
using CustomCommands.Configuration;
using CustomCommands.Configuration.AppSettings;
using CustomCommands.Events;
using Microsoft.CognitiveServices.Speech;

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
            
            Console.WriteLine("Write a command or say OK SPEAKER and speech your command");
            
            var t1 = StartListenForCommand(keywordRecognizerClient, customEventHandlers);

            var t2 = StartWaitForWrittenCommand(customEventHandlers);

            await Task.WhenAny(t1, t2);
        }

        private static async Task StartListenForCommand(SpeechRecognizerClient keywordRecognizerClient,
            CustomCommandHandler customEventHandlers)
        {
            await Task.Run( async () =>
            {
                while (true)
                {
                    // NeverEnded listening for keyword
                    await keywordRecognizerClient.StartRecognitionWithKeywordRecognizer("OK_SPEAKER",
                        "Configuration/Keyword/okSpeakerKwd.table");

                    // Delay to avoid auto-listening
                    await Task.Delay(50);

                    // Utterance limited listening for commands
                    await customEventHandlers.Client.StartListenForSpeech();
                }
            });
        }


        private static async Task StartWaitForWrittenCommand(CustomCommandHandler customEventHandlers)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var test = Console.ReadLine();
                    
                    if (test is not null)
                    {
                        await customEventHandlers.Client.SendActivity(test);
                    }
                }
            });
        }
        
        private static SpeechSynthesizer CreateSynthesizer(CustomCommandClientConfiguration configuration)
        {
            return new(SpeechConfig.FromSubscription(configuration.SubscriptionKey, configuration.Region));
        }
    }
}
