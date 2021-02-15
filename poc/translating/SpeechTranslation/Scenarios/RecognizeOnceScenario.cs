using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace SpeechTranslation
{
    class RecognizeOnceScenario : AbstractScenario
    {
        public RecognizeOnceScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        /// <summary>
        /// https://aka.ms/speech/sttt-languages
        /// </summary>
        public async Task Run(string inputLanguage, IReadOnlyList<string> targetLanguages)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
            translationConfig.SpeechRecognitionLanguage = inputLanguage;

            foreach (var language in targetLanguages)
                translationConfig.AddTargetLanguage(language);

            using var recognizer = new TranslationRecognizer(translationConfig);

            Console.WriteLine($"Say something in '{inputLanguage}'...");

            TranslationRecognitionResult result;

            do
            {
                result = await recognizer.RecognizeOnceAsync();
            } while (result.Reason != ResultReason.TranslatedSpeech);

            Console.WriteLine();
            Console.WriteLine($"'{result.Text}'");

            foreach (var (language, translation) in result.Translations)
                Console.WriteLine($"  [{language}] '{translation}'");
        }
    }
}
