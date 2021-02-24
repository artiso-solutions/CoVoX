using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using Shared;

namespace SpeechTranslation
{
    class ContinuousRecognitionPerfScenario : AbstractScenario
    {
        public ContinuousRecognitionPerfScenario(string subscriptionKey, string region, bool logScenarioName = true)
            : base(subscriptionKey, region, logScenarioName)
        {
        }

        public async Task Run(
            string inputLanguage,
            IReadOnlyList<string> targetLanguages,
            Shared.OutputFormat outputFormat = Shared.OutputFormat.Csv,
            int iterations = 1,
            bool helperLogs = true,
            CancellationToken cancellationToken = default)
        {
            var collector = new DataCollector(inputLanguage, outputFormat);

            var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
            translationConfig.SpeechRecognitionLanguage = inputLanguage;

            foreach (var language in targetLanguages)
                translationConfig.AddTargetLanguage(language);

            using var recognizer = new TranslationRecognizer(translationConfig);
            var currentIteration = 0;

            recognizer.Recognizing += (_, args) =>
            {
                var result = args.Result;
                if (string.IsNullOrEmpty(result.Text)) return;
                collector.RegisterEvent($"{result.Reason}...", result.Text);
            };

            recognizer.Recognized += (_, args) =>
            {
                var result = args.Result;
                if (string.IsNullOrEmpty(result.Text)) return;

                if (helperLogs)
                    Console.WriteLine("-- recognized --");

                if (result.Reason == ResultReason.TranslatedSpeech)
                {
                    collector.RegisterEvent("Translated", result.Text);

                    currentIteration++;

                    if (currentIteration == iterations)
                    {
                        currentIteration = 0;
                        collector.Render();
                    }
                    else
                    {
                        collector.RegisterSeparator();
                        var remaining = iterations - currentIteration;
                        Console.WriteLine($"{remaining} remaining iterations...");
                    }

                    collector.RestartTime();
                }
                else
                    collector.RegisterEvent(result.Reason.ToString(), result.Text);
            };

            recognizer.Canceled += (_, _) => collector.RegisterEvent("Cancelled");

            recognizer.SpeechStartDetected += (_, _) => collector.RegisterEvent("SpeechStartDetected");

            recognizer.SpeechEndDetected += (_, _) => collector.RegisterEvent("SpeechEndDetected");

            // Start

            await recognizer.StartContinuousRecognitionAsync();

            if (helperLogs)
            {
                Console.WriteLine($"Say something in '{inputLanguage}'...");
                Console.WriteLine();
            }

            collector.RestartTime();

            // Setup cancellation

            await cancellationToken.AsTask();
            await recognizer.StopContinuousRecognitionAsync();
        }
    }
}
