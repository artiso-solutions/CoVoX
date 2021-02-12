using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace SpeechToTextFromMic
{
    class Program
    {
        private static readonly string SubscriptionKey = "SubscriptionKey";
        private static readonly string Region = "Region";
        private static readonly AutoDetectSourceLanguageConfig AutoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US", "de-DE", "es-ES", "it-IT" });

        static async Task Main(string[] args)
        {
            var showMenu = true;
            while (showMenu)
            {
                showMenu = await Menu();
            }
        }

        private static async Task<bool> Menu()
        {
            Console.Clear();
            Console.WriteLine("Choose recognition mode:");
            Console.WriteLine("1) Single-shot");
            Console.WriteLine("2) Continuous");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("I'm listening...");
                    var result = await SpeechRecognition.RecognizeSpeech(SubscriptionKey, Region, AutoDetectSourceLanguageConfig);
                    Console.WriteLine($"Recognized '{result.DetectedLanguage}': {result.Text}");
                    Console.Write("\r\nPress Enter to return to the menu");
                    Console.ReadLine();

                    return true;
                case "2":
                    await SpeechRecognition.RecognizeSpeechContinuous(SubscriptionKey, Region, AutoDetectSourceLanguageConfig);
                    Console.ReadKey();
                    await SpeechRecognition.Recognizer.StopContinuousRecognitionAsync();
                    return true;
                default:
                    return false;
            }
        }
    }
}
