using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace LoopRecognizeOnce
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = SpeechConfig.FromSubscription("SpeechSubscriptionKey", "Region");
            var recognizer = new SpeechRecognizer(config);

            while (true)
            {
                Console.WriteLine("I'm listening...");

                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.Canceled)
                    throw new Exception("Api Communication Failure");

                var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;

                Console.WriteLine($"{detectedLanguage}: {result.Text}");
            }
        }
    }
}
