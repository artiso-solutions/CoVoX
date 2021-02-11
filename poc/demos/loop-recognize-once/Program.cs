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
                var startTime = DateTime.Now;

                Console.WriteLine("I'm listening...");

                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.Canceled)
                    throw new Exception("Api Communication Failure");

                var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;

                if (result.Text != "")
                {
                    Console.WriteLine($"{detectedLanguage}: {result.Text} ({(DateTime.Now - startTime).TotalMilliseconds} ms)");
                }
            }
        }
    }
}
