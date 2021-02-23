using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Translating;
using API.Understanding;

namespace API
{
    public delegate void CommandRecognized(Command command, RecognitionContext context);

    internal class RecognitionLoop
    {
        private readonly IMultiLanguageTranslatingModule _translationModule;
        private readonly IUnderstandingModule _understandingModule;
        private CancellationTokenSource _cts;
        private Task _loop;

        public RecognitionLoop(
            IMultiLanguageTranslatingModule translationModule,
            IUnderstandingModule understandingModule)
        {
            _translationModule = translationModule;
            _understandingModule = understandingModule;
        }

        public bool IsActive { get; private set; }

        public event CommandRecognized Recognized;

        public async Task StartAsync()
        {
            if (IsActive) return;
            IsActive = true;

            _cts = new CancellationTokenSource();
            _loop = Task.Run(() => ContinuousRecognitionAsync(_cts.Token), _cts.Token);
            await _translationModule.StartAsync();
        }

        public async Task StopAsync()
        {
            if (!IsActive) return;
            IsActive = false;

            _cts.Cancel();
            await _translationModule.StopAsync();
            
            await _loop;
            _loop = null;
        }

        private async Task ContinuousRecognitionAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
                await RecognizeAsync(cancellationToken);
        }

        private async Task RecognizeAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var recognitions = await _translationModule.RecognizeOneOfEachAsync(cancellationToken);
            if (recognitions is null || !recognitions.Any()) return;

            Match bestMatch = null;
            RecognitionContext context = null;

            foreach (var recognition in recognitions.Where(x => x.input is not null))
            {
                var (input, inputLanguage) = recognition;
                var (match, candidates) = _understandingModule.Understand(input);

                if (match is null) continue;

                if (bestMatch is null || bestMatch.MatchScore < match.MatchScore)
                {
                    bestMatch = match;
                    context = new RecognitionContext(input, inputLanguage, match.MatchScore, candidates);
                }
            }

            if (bestMatch is not null && context is not null)
                Recognized?.Invoke(bestMatch.Command, context);
        }
    }
}
