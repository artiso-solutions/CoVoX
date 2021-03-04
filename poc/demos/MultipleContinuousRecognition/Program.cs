using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpeechToTextFromMic;

namespace MultipleContinuousRecognition
{
    internal static class Program
    {
        private const string SubscriptionKey = "0000";
        private const string Region = "0000";

        private static async Task Main()
        {
            var speechRecognizer = new SpeechRecognition(SubscriptionKey, Region, new List<string>()
            {
                "en-US",
                "de-DE",
                "it-IT",
                "es-ES"
            });
            
            await ParallelRecognize(speechRecognizer);
            
            Console.ReadLine();
        }
        
        private static async Task ParallelRecognize(SpeechRecognition speechRecognition)
        {
            try
            {
                var recognizeEn = RecognizeContinuous(speechRecognition, "en-US");
                var recognizeDe = RecognizeContinuous(speechRecognition,"de-DE");
                var recognizeIt = RecognizeContinuous(speechRecognition,"it-IT");
                var recognizeEs = RecognizeContinuous(speechRecognition,"es-ES");

                Console.WriteLine("I'm listening...");

                await Task.WhenAll(recognizeEn
                    ,recognizeDe 
                    ,recognizeEs 
                    ,recognizeIt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        private static async Task RecognizeContinuous(SpeechRecognition speechRecognition, string language)
        {   
            await speechRecognition.RecognizeSpeechContinuous(language);
        }
    }
}
