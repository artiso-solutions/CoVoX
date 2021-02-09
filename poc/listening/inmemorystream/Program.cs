using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToTextFromInMemoryStream
{
    class Program
    {
        private static readonly string SubscriptionKey = "407e84e6aba84e53b646e74f7d25d687";
        private static readonly string Region = "westeurope";
        
        static async Task Main(string[] args)
        {
            var reader = new BinaryReader(File.OpenRead("whatstheweatherlike.wav"));
            using var audioInputStream = AudioInputStream.CreatePushStream();

            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioInputStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var result = await SpeechRecognition.RecognizeSpeechFromStream(SubscriptionKey, Region, audioInputStream);
            Console.WriteLine($"Recognized '{result.DetectedLanguage}': {result.Text}");
            Console.ReadLine();
        }
    }
}
