using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using SpeechToTextFromMic;
using TextTranslate;

namespace MultipleRecognizeOnce
{
    class Program
    {
        private static readonly string SpeechSubscriptionKey = "SpeechSubscriptionKey";
        private static readonly string TranslatorKey = "TranslatorKey";
        private static readonly string Region = "Region";

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Write("Press a key to start");
                Console.ReadLine();
                await ParallelRecognize();
            }
        }
        public static async Task ParallelRecognize()
        {
            var recognizeEn = SpeechRecognition.RecognizeSpeech(SpeechSubscriptionKey, Region, AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US" }));
            var recognizeDe = SpeechRecognition.RecognizeSpeech(SpeechSubscriptionKey, Region, AutoDetectSourceLanguageConfig.FromLanguages(new[] { "de-DE" }));
            var recognizeIt = SpeechRecognition.RecognizeSpeech(SpeechSubscriptionKey, Region, AutoDetectSourceLanguageConfig.FromLanguages(new[] { "it-IT" }));
            var recognizeEs = SpeechRecognition.RecognizeSpeech(SpeechSubscriptionKey, Region, AutoDetectSourceLanguageConfig.FromLanguages(new[] { "es-ES" }));
            
            Console.WriteLine("I'm listening...");
            
            var results = await Task.WhenAll(recognizeEn, recognizeDe, recognizeEs, recognizeIt);

            foreach (var result in results)
            {
                Console.WriteLine($"{result.DetectedLanguage}: {result.Text}");
                var translation = TextTranslation.TranslateToEn(TranslatorKey, Region, result.Text).Result.FirstOrDefault();
                Console.WriteLine($"{translation?.Translations.FirstOrDefault()?.Text} (Translated from: {translation?.DetectedLanguage.Language} - Score: {translation?.DetectedLanguage.Score * 100}%");
                Console.WriteLine();
            }
        }
    }
}
