﻿using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace SpeechTranslation
{
    class TranslateToVoiceScenario : AbstractScenario
    {
        public TranslateToVoiceScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        [SupportedOSPlatform("windows")]
        public async Task Run(
            string inputLanguage,
            CancellationToken cancellationToken = default)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
            translationConfig.SpeechRecognitionLanguage = inputLanguage;

            // Voices: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#standard-voices
            translationConfig.VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
            translationConfig.AddTargetLanguage("en-US");

            using var recognizer = new TranslationRecognizer(translationConfig);

            recognizer.Synthesizing += (s, e) =>
            {
                var audio = e.Result.GetAudio();

                if (audio.Length > 0)
                {
                    using var m = new MemoryStream(audio);
                    var player = new SoundPlayer(m);
                    player.PlaySync();
                }
            };

            cancellationToken.Register(() => recognizer.StopContinuousRecognitionAsync().Wait());

            Console.WriteLine($"Say something in '{inputLanguage}'...");
            Console.WriteLine();

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await recognizer.RecognizeOnceAsync();
                var (_, translation) = result.Translations.First();

                if (!string.IsNullOrEmpty(translation))
                {
                    Console.WriteLine();
                    Console.WriteLine($"You: '{result.Text}'");
                    Console.WriteLine($"Zira: '{translation}'");
                }
            }
        }
    }
}
