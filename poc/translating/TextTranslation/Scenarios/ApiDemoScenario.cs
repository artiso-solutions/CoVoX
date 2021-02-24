using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace TextTranslate
{
    class ApiDemoScenario : AbstractScenario
    {
        public ApiDemoScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Input text to translate to english: ");
                var textToTranslate = Console.ReadLine();
                var results = await TextTranslation.TranslateToEn(SubscriptionKey, Region, textToTranslate);
                
                foreach (var result in results)
                {
                    Console.WriteLine($"Detected language: {result.DetectedLanguage.Language}");
                    Console.WriteLine($"Confidence score: {result.DetectedLanguage.Score}");
                    Console.WriteLine($"Translated: {result.Translations.First().Text}");
                    Console.WriteLine();
                }
            }
        }
    }
}
