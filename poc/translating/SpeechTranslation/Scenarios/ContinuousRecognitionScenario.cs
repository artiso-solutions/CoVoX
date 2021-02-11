using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace SpeechTranslation
{
    class ContinuousRecognitionScenario : AbstractScenario
    {
        public ContinuousRecognitionScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        public async Task Run(
            string inputLanguage,
            IReadOnlyList<string> targetLanguages,
            CancellationToken cancellationToken = default)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
            translationConfig.SpeechRecognitionLanguage = inputLanguage;

            foreach (var language in targetLanguages)
                translationConfig.AddTargetLanguage(language);

            using var recognizer = new TranslationRecognizer(translationConfig);

            recognizer.Recognizing += (_, args) =>
            {
                var result = args.Result;
                Console.WriteLine($"(recognizing...) '{result.Text}'");
            };

            recognizer.Recognized += (_, args) =>
            {
                var result = args.Result;
                
                Console.WriteLine($"'{result.Text}'");
                
                foreach (var (language, translation) in result.Translations)
                    Console.WriteLine($"  [{language}] '{translation}'");

                Console.WriteLine();
            };

            await recognizer.StartContinuousRecognitionAsync();

            Console.WriteLine($"Say something in '{inputLanguage}'...");
            Console.WriteLine();

            // Setup cancellation

            cancellationToken.Register(() => recognizer.StopContinuousRecognitionAsync().Wait());

            await cancellationToken.AsTask();
        }
    }
}
