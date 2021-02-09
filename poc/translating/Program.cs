using System;
using System.Linq;
using System.Threading.Tasks;

namespace TextTranslate
{
    class Program
    {
        private static readonly string SubscriptionKey = "407e84e6aba84e53b646e74f7d25d687";
        private static readonly string Region = "westeurope";

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Write("Input text to translate to english: ");
                var textToTranslate = Console.ReadLine();
                var results = await TextTranslation.TranslateToEn(SubscriptionKey, Region, textToTranslate);
                foreach (var result in results)
                {
                    Console.WriteLine($"Detected language: {result.DetectedLanguage.Language}");
                    Console.WriteLine($"Confidence score: {result.DetectedLanguage.Score}");
                    Console.WriteLine($"Translated: {result.Translations.First().Text}");
                }
            }
        }
    }
}
