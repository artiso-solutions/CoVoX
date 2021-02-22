using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Modules
{
    public delegate void LanguageRecognized(string text, string inputLanguage);

    internal interface IMultiLanguageTranslatingModule
    {
        bool IsActive { get; }

        event LanguageRecognized Recognized;

        Task<(string input, string inputLanguage)> RecognizeOneAsync(CancellationToken cancellationToken);

        Task<IReadOnlyList<(string input, string inputLanguage)>> RecognizeOneOfEachAsync(CancellationToken cancellationToken);

        Task StartAsync();

        Task StopAsync();
    }
}
