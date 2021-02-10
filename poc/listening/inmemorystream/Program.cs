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
            var showMenu = true;
            while (showMenu)
            {
                showMenu = await Menu();
            }
        }

        private static async Task<bool> Menu()
        {
            Console.Clear();
            Console.WriteLine("Choose type of in-memory stream:");
            Console.WriteLine("1) Pre-filled");
            Console.WriteLine("2) Post-filled");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await RecognizePreFilledStream();
                    Console.Write("\r\nPress Enter to return to the menu");
                    Console.ReadLine();
                    return true;
                case "2":
                    await RecognizePostFilledStream();
                    Console.Write("\r\nPress Enter to return to the menu");
                    Console.ReadLine();
                    return true;
                default:
                    return false;
            }
        }

        private static async Task RecognizePreFilledStream()
        {
            var reader = new BinaryReader(File.OpenRead("whatstheweatherlike.wav"));
            var audioInputStream = AudioInputStream.CreatePushStream();

            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioInputStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var result = await SpeechRecognition.RecognizeSpeechFromStream(SubscriptionKey, Region, audioInputStream, AutoDetectSourceLanguageConfig);
            Console.WriteLine($"Recognized '{result.DetectedLanguage}': {result.Text}");
        }

        private static async Task RecognizePostFilledStream()
        {
            var speechConfig = SpeechConfig.FromSubscription(SubscriptionKey, Region);
            
            var audioInputStream = AudioInputStream.CreatePushStream();
            
            using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
            using var recognizer = new SpeechRecognizer(speechConfig, AutoDetectSourceLanguageConfig, audioConfig);

            var recognitionTask = recognizer.RecognizeOnceAsync();

            await Task.Delay(TimeSpan.FromSeconds(10));

            var fillAudioStreamTask = Task.Run(() =>
            {
                var reader = new BinaryReader(File.OpenRead("whatstheweatherlike.wav"));

                byte[] readBytes;
                do
                {
                    readBytes = reader.ReadBytes(1024);
                    audioInputStream.Write(readBytes, readBytes.Length);
                } while (readBytes.Length > 0);
            });

            await Task.WhenAll(recognitionTask, fillAudioStreamTask);

            if (recognitionTask.Result.Reason == ResultReason.Canceled)
                throw new Exception("Api Communication Failure");

            var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(recognitionTask.Result).Language;
            
            Console.WriteLine($"Recognized '{detectedLanguage}': {recognitionTask.Result.Text}");
        }
    }
}
