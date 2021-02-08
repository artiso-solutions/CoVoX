using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToTextFromInMemoryStream
{
    class Program
    {
        private static readonly string SubscriptionKey = "SubscriptionKey";
        private static readonly string Region = "Region";
        private static readonly AutoDetectSourceLanguageConfig AutoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US", "de-DE", "es-ES", "it-IT" });


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

            var result = await SpeechRecognition.RecognizeSpeechFromStream(SubscriptionKey, Region, audioInputStream, AutoDetectSourceLanguageConfig);
            Console.WriteLine($"Recognized '{result.DetectedLanguage}': {result.Text}");
            Console.ReadLine();
        }
    }
}
