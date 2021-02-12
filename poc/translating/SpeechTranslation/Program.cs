using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

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
            await RunTranslateToVoiceScenario();
        }

        public static async Task RunRecognizeOnceScenario()
        {
            await new RecognizeOnceScenario(SubscriptionKey, Region).Run(
                inputLanguage: "it-IT",
                targetLanguages: new[] { "en-US", "en-UK", "de-DE" });
        }

        public static async Task RunContinuousRecognitionScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await new ContinuousRecognitionScenario(SubscriptionKey, Region).Run(
                inputLanguage: "it-IT",
                targetLanguages: new[] { "en-US", "en-UK", "de-DE" },
                cancellationToken);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public static async Task RunTranslateToVoiceScenario()
        {
            var cancellationToken = Utils.GetUserCancellableToken();

            await new TranslateToVoiceScenario(SubscriptionKey, Region).Run(
                inputLanguage: "it-IT",
                cancellationToken);
        }
    }
}
