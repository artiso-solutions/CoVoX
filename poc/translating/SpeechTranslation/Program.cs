using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Shared;

namespace SpeechTranslation
{
    class Program
    {
        private static readonly string SubscriptionKey = "";
        private static readonly string Region = "";

        static async Task Main()
        {
            if (string.IsNullOrWhiteSpace(SubscriptionKey)) throw new ArgumentNullException(nameof(SubscriptionKey));
            if (string.IsNullOrWhiteSpace(Region)) throw new ArgumentNullException(nameof(Region));

            // Languages: https://aka.ms/speech/sttt-languages

            //await RunRecognizeOnceScenario();
            //await RunContinuousRecognitionScenario();
            //await RunTranslateToVoiceScenario();
            //await RunContinuousRecognitionPerfScenario();
            await RunMultipleContinuousRecognitionPerfScenario();
        }

        public static async Task RunRecognizeOnceScenario()
        {
            await new RecognizeOnceScenario(SubscriptionKey, Region).Run(
                inputLanguage: "en-US",
                targetLanguages: new[] { "it-IT", "de-DE", "es-ES" });
        }

        public static async Task RunContinuousRecognitionScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await new ContinuousRecognitionScenario(SubscriptionKey, Region).Run(
                inputLanguage: "en-US",
                targetLanguages: new[] { "it-IT", "de-DE", "es-ES" },
                cancellationToken);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public static async Task RunTranslateToVoiceScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await new TranslateToVoiceScenario(SubscriptionKey, Region).Run(
                inputLanguage: "en-US",
                cancellationToken);
        }

        public static async Task RunContinuousRecognitionPerfScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await new ContinuousRecognitionPerfScenario(SubscriptionKey, Region).Run(
                inputLanguage: "en-US",
                targetLanguages: new[] { "it-IT", "de-DE", "es-ES" },
                outputFormat: OutputFormat.Table,
                cancellationToken: cancellationToken);
        }

        public static async Task RunMultipleContinuousRecognitionPerfScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await Task.WhenAll(
                new ContinuousRecognitionPerfScenario(SubscriptionKey, Region, false).Run(
                    inputLanguage: "en-US",
                    targetLanguages: new[] { "it-IT", "de-DE", "es-ES" },
                    outputFormat: OutputFormat.Markdown,
                    iterations: 3,
                    false,
                    cancellationToken),

                new ContinuousRecognitionPerfScenario(SubscriptionKey, Region, false).Run(
                    inputLanguage: "it-IT",
                    targetLanguages: new[] { "en-US", "de-DE", "es-ES" },
                    outputFormat: OutputFormat.Markdown,
                    iterations: 3,
                    false,
                    cancellationToken),

                new ContinuousRecognitionPerfScenario(SubscriptionKey, Region, false).Run(
                    inputLanguage: "de-DE",
                    targetLanguages: new[] { "it-IT", "en-US", "es-ES" },
                    outputFormat: OutputFormat.Markdown,
                    iterations: 3,
                    false,
                    cancellationToken),

                new ContinuousRecognitionPerfScenario(SubscriptionKey, Region, false).Run(
                    inputLanguage: "es-ES",
                    targetLanguages: new[] { "it-IT", "de-DE", "en-US" },
                    outputFormat: OutputFormat.Markdown,
                    iterations: 3,
                    false,
                    cancellationToken)
                );
        }
    }
}
