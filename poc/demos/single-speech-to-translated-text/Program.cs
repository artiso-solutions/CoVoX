using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using SpeechToTextFromMic;
using TextTranslate;

namespace SingleSTTT
{
    class Program
    {
        private static readonly string SpeechSubscriptionKey = "SpeechSubscriptionKey";
        private static readonly string TranslatorKey = "TranslatorKey";
        private static readonly string Region = "Region";
        private static readonly AutoDetectSourceLanguageConfig AutoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US", "de-DE", "es-ES", "it-IT" });

        static async Task Main(string[] args)
        {
            await Recognize(SpeechSubscriptionKey, TranslatorKey, Region, AutoDetectSourceLanguageConfig);
            Console.ReadLine();
        }

        private static async Task Recognize(string subscriptionKey, string translatorKey, string region, AutoDetectSourceLanguageConfig autoDetectSourceLanguageConfig)
        {
            Console.WriteLine("I'm listening...");
            var speechRecognitionResult = await SpeechRecognition.RecognizeSpeech(subscriptionKey, region, autoDetectSourceLanguageConfig);
            Console.WriteLine($"Detected: {speechRecognitionResult.Text}");
            Console.WriteLine(speechRecognitionResult.DetectedLanguage == "en-US"
                ? "English detected, no need to translate"
                : $"Translated: {await TextTranslation.TranslateToEn(translatorKey, region, speechRecognitionResult.Text)}");
        }
    }
}
