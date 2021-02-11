using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using SpeechToTextFromMic;
using TextTranslate;

namespace MultipleRecognizeOnceLoop
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
                await ParallelRecognize();
            }
        }
        public static async Task ParallelRecognize()
        {
            try
            {
                var recognizeEn = Recognize(SpeechSubscriptionKey, TranslatorKey, Region, "en-US");
                var recognizeDe = Recognize(SpeechSubscriptionKey, TranslatorKey, Region, "de-DE");
                var recognizeIt = Recognize(SpeechSubscriptionKey, TranslatorKey, Region, "it-IT");
                var recognizeEs = Recognize(SpeechSubscriptionKey, TranslatorKey, Region, "es-ES");

                Console.WriteLine("I'm listening...");

                await Task.WhenAll(recognizeEn, recognizeDe, recognizeEs, recognizeIt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task Recognize(string subscriptionKey, string translatorKey, string region, string language)
        {
            var startTime = DateTime.Now;
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { language });
            var (text, detectedLanguage) = await SpeechRecognition.RecognizeSpeech(subscriptionKey, region, autoDetectSourceLanguageConfig);
            if (text != "" && language.Contains(detectedLanguage))
            {
                var translationResult = await TextTranslation.TranslateToEn(translatorKey, region, text);
                var translation = translationResult.FirstOrDefault();
                Console.WriteLine($"{language} - Detected: {text} - Translated: {translation.Translations.FirstOrDefault().Text} ({translation.DetectedLanguage.Language}) ({(DateTime.Now - startTime).TotalMilliseconds} ms)");
            }
        }
    }
}
