using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Covox.Translating
{
    public delegate void LanguageRecognized(string text, string inputLanguage);

    public delegate void LanguageRecognizing(string input, string inputLanguage, string translation);

    internal interface IMultiLanguageTranslatingModule
    {
        bool IsActive { get; }

        event LanguageRecognized Recognized;

        event LanguageRecognizing Recognizing;

        Task<(string input, string inputLanguage)> RecognizeOneAsync(CancellationToken cancellationToken);

        Task<IReadOnlyList<(string input, string inputLanguage)>> RecognizeOneOfEachAsync(CancellationToken cancellationToken);

        Task StartAsync();

        Task StopAsync();
    }
}
