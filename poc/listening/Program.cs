using System;
using System.Threading.Tasks;

namespace SpeechToText
{
    class Program
    {
        private static readonly string SubscriptionKey = "SubscriptionKey";
        private static readonly string Region = "Region";
        
        static async Task Main(string[] args)
        {
            await SpeechRecognition.RecognizeSpeechContinuous(SubscriptionKey, Region);
            Console.ReadLine();
        }  
    }
}
