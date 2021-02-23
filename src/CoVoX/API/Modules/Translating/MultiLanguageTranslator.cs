using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Translating
{
    internal class MultiLanguageTranslator : IMultiLanguageTranslatingModule, IAsyncDisposable
    {
        private readonly IReadOnlyList<Translator> _translators;
        private readonly AutoResetTaskCompletionSource<(string input, string inputLanguage)> _atcs = new();

        public MultiLanguageTranslator(
            AzureConfiguration azureConfiguration,
            IReadOnlyList<string> inputLanguages)
        {
            _translators = inputLanguages.Select(lang => new Translator(azureConfiguration, lang)).ToArray();

            foreach (var translator in _translators)
                translator.Recognized += text => OnRecognized(text, translator.InputLanguage);
        }

        public bool IsActive { get; private set; }

        public event LanguageRecognized Recognized;

        private void OnRecognized(string input, string inputLanguage)
        {
            _ = _atcs.TrySetResult((input, inputLanguage));
            Recognized?.Invoke(input, inputLanguage);
        }

        public async Task<(string input, string inputLanguage)> RecognizeOneAsync(CancellationToken cancellationToken)
        {
            using var cancellationSource = new CancellationTokenTaskSource<(string, string)>(cancellationToken);

            var timeoutTask = cancellationSource.Task;
            var recognitionTask = _atcs.Task;
            await Task.WhenAny(recognitionTask, timeoutTask);

            if (cancellationToken.IsCancellationRequested)
                return default;

            var (input, inputLanguage) = await recognitionTask;
            return (input, inputLanguage);
        }

        public async Task<IReadOnlyList<(string input, string inputLanguage)>> RecognizeOneOfEachAsync(CancellationToken cancellationToken)
        {
            using var cancellationSource = new CancellationTokenTaskSource<(string, string)>(cancellationToken);

            var timeoutTask = cancellationSource.Task;
            var recognitionTasks = _translators.Select(t => RecognizeOneWithLanguageAsync(t, cancellationToken));
            var recognitionsCompletedTask = Task.WhenAll(recognitionTasks);

            await Task.WhenAny(recognitionsCompletedTask, timeoutTask);

            var completedTasks = recognitionTasks.Where(t => t.IsCompletedSuccessfully);
            var results = await Task.WhenAll(completedTasks);

            return results;
        }

        private static async Task<(string input, string inputLanguage)> RecognizeOneWithLanguageAsync(
            Translator translator,
            CancellationToken cancellationToken)
        {
            var text = await translator.RecognizeOneAsync(cancellationToken);
            return (text, translator.InputLanguage);
        }

        public async Task StartAsync()
        {
            IsActive = true;
            await Task.WhenAll(_translators.Select(t => t.StartAsync()));
        }

        public async Task StopAsync()
        {
            IsActive = false;
            await Task.WhenAll(_translators.Select(t => t.StopAsync()));
        }

        public async ValueTask DisposeAsync() =>
            await Task.WhenAll(_translators.Select(t => t.DisposeAsync().AsTask()));
    }
}
