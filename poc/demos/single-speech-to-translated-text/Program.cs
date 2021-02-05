using System;
using System.Threading.Tasks;
using SpeechToTextFromMic;
using TextTranslate;

namespace SingleSTTT
{
    class Program
    {
        private static readonly string SpeechSubscriptionKey = "SpeechSubscriptionKey";
        private static readonly string TranslatorKey = "TranslatorKey";
        private static readonly string Region = "Region";
        
        static async Task Main(string[] args)
        {
            await Recognize(SpeechSubscriptionKey, TranslatorKey, Region);
            Console.ReadLine();
        }

        private static async Task Recognize(string subscriptionKey, string translatorKey, string region)
        {
            Console.WriteLine("I'm listening...");
            var speechRecognitionResult = await SpeechRecognition.RecognizeSpeech(subscriptionKey, region);
            Console.WriteLine($"Detected: {speechRecognitionResult.Text}");
            Console.WriteLine(speechRecognitionResult.DetectedLanguage == "en-US"
                ? "English detected, no need to translate"
                : $"Translated: {await TextTranslation.TranslateToEn(translatorKey, region, speechRecognitionResult.Text)}");
        }
    }
}
