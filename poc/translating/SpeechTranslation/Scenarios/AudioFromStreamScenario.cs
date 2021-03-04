using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Shared;

namespace SpeechTranslation
{
    class AudioFromStreamScenario : AbstractScenario
    {
        public AudioFromStreamScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        public async Task Run(string inputLanguage, IReadOnlyList<string> targetLanguages)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
            translationConfig.SpeechRecognitionLanguage = inputLanguage;

            foreach (var language in targetLanguages)
                translationConfig.AddTargetLanguage(language);

            var audioInputStream = AudioInputStream.CreatePushStream();
            using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
            using var recognizer = new TranslationRecognizer(translationConfig, audioConfig);

            var fillingTask = FillStream(audioInputStream);

            TranslationRecognitionResult result;

            do
            {
                result = await recognizer.RecognizeOnceAsync();
            } while (result.Reason != ResultReason.TranslatedSpeech);

            Console.WriteLine();
            Console.WriteLine($"'{result.Text}'");

            foreach (var (language, translation) in result.Translations)
                Console.WriteLine($"  [{language}] '{translation}'");
        }

        private static async Task FillStream(PushAudioInputStream audioInputStream)
        {
            await Task.Delay(TimeSpan.FromSeconds(2)); // Delayed audio

            Console.WriteLine("**Writing to audio stream**");
            var reader = new BinaryReader(File.OpenRead("whatstheweatherlike.wav"));

            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioInputStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);
        }
    }
}
