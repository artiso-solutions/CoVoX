using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace API.Modules
{
    internal class Translator : ITranslatingModule, IAsyncDisposable
    {
        private readonly TranslationRecognizer _recognizer;
        private readonly AutoResetTaskCompletionSource<string> _atcs = new();

        public Translator(AzureConfiguration azureConfiguration, string inputLanguage)
            : this(azureConfiguration, inputLanguage, targetLanguage: "en-US")
        {
        }

        private Translator(
            AzureConfiguration azureConfiguration,
            string inputLanguage,
            string targetLanguage)
        {
            InputLanguage = inputLanguage;
            TargetLanguage = targetLanguage;

            var recognizerConfig = GetRecognizerConfig(azureConfiguration, inputLanguage, targetLanguage);
            _recognizer = new TranslationRecognizer(recognizerConfig);
            _recognizer.Recognized += (_, args) =>
            {
                var translatedText = args.Result.Translations.Values.FirstOrDefault();
                OnRecognized(translatedText);
            };
        }

        public string InputLanguage { get; }

        public string TargetLanguage { get; }

        public bool IsActive { get; private set; }

        public event TextRecognized Recognized;

        private static SpeechTranslationConfig GetRecognizerConfig(
            AzureConfiguration azureConfiguration,
            string inputLanguage,
            string targetLanguage)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(
                azureConfiguration.SubscriptionKey, azureConfiguration.Region);

            translationConfig.SpeechRecognitionLanguage = inputLanguage;
            translationConfig.AddTargetLanguage(targetLanguage);
            translationConfig.SetProfanity(ProfanityOption.Raw);
            return translationConfig;
        }

        private void OnRecognized(string text)
        {
            _ = _atcs.TrySetResult(text);
            Recognized?.Invoke(text);
        }

        public async Task StartAsync()
        {
            IsActive = true;
            await _recognizer.StartContinuousRecognitionAsync();
        }

        public async Task StopAsync()
        {
            IsActive = false;
            await _recognizer.StopContinuousRecognitionAsync();
        }

        public async Task<string> RecognizeOneAsync(CancellationToken cancellationToken)
        {
            using var cancellationSource = new CancellationTokenTaskSource<string>(cancellationToken);

            var timeoutTask = cancellationSource.Task;
            var recognitionTask = _atcs.Task;
            await Task.WhenAny(recognitionTask, timeoutTask);

            if (cancellationToken.IsCancellationRequested)
                return default;

            var text = await recognitionTask;
            return text;
        }

        public async ValueTask DisposeAsync() => await StopAsync();
    }
}
