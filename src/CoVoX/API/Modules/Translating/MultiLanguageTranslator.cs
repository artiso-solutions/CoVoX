using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Covox.Translating
{
    internal class MultiLanguageTranslator : IMultiLanguageTranslatingModule, IExposeErrors, IAsyncDisposable
    {
        private readonly IReadOnlyList<Translator> _translators;
        private readonly AutoResetTaskCompletionSource<(string input, string inputLanguage)> _atcs = new();

        internal MultiLanguageTranslator(
            AzureConfiguration azureConfiguration,
            IReadOnlyList<string> inputLanguages)
        {
            _translators = inputLanguages.Select(lang => new Translator(azureConfiguration, lang)).ToArray();

            foreach (var translator in _translators)
            {
                translator.Recognized += text => OnRecognized(text, translator.InputLanguage);
                translator.OnError += ex => OnTranslatorError(ex);
            }
        }

        public bool IsActive { get; private set; }

        public event LanguageRecognized? Recognized;

        public event ErrorHandler? OnError;

        private void OnRecognized(string input, string inputLanguage)
        {
            _ = _atcs.TrySetResult((input, inputLanguage));
            Recognized?.Invoke(input, inputLanguage);
        }

        private void OnTranslatorError(Exception ex)
        {
            _ = _atcs.TrySetException(ex);
            OnError?.Invoke(ex);
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
            var recognitionTasks = _translators.Select(t => RecognizeOneWithLanguageAsync(t, cancellationToken)).ToArray();
            var recognitionsCompletedTask = Task.WhenAll(recognitionTasks);

            var completedTask = await Task.WhenAny(recognitionsCompletedTask, timeoutTask);

            if (completedTask == timeoutTask)
                return Array.Empty<(string input, string inputLanguage)>();

            var completedRecognitions = recognitionTasks
                .Where(t => t.IsCompletedSuccessfully)
                .Select(t => t.Result)
                .ToArray();

            var nonEmptyResults = completedRecognitions.Where(x =>
                !string.IsNullOrWhiteSpace(x.input) &&
                !string.IsNullOrWhiteSpace(x.inputLanguage)).ToArray();

            if (nonEmptyResults.Any())
                return nonEmptyResults!;

            return Array.Empty<(string input, string inputLanguage)>();
        }

        private static async Task<(string? input, string? inputLanguage)> RecognizeOneWithLanguageAsync(
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
