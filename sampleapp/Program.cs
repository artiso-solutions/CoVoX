using System;
using System.Threading.Tasks;
using SpeechToText;
using TextTranslate;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                await Recognize();
            }
        }

        private static async Task Recognize()
        {
            Console.WriteLine("Press <Return> to start recognition");
            if (Console.ReadKey().Key != ConsoleKey.Enter) return;

            Console.WriteLine("I'm listening...");
            var speechRecognitionResult = SpeechRecognition.RecognizeSpeech();
            Console.WriteLine($"Detected: {speechRecognitionResult.Text}");
            Console.WriteLine(speechRecognitionResult.DetectedLanguage == "en-US"
                ? "English detected, no need to translate"
                : $"Translated: {await TextTranslation.TranslateToEn(speechRecognitionResult.Text)}");
        }
    }
}
