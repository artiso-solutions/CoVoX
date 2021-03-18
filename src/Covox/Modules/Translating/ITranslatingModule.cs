using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Covox.Translating
{
    public delegate void TextRecognized(string text);

    public delegate void TextRecognizing(string input, IReadOnlyDictionary<string, string> translations);

    internal interface ITranslatingModule
    {
        bool IsActive { get; }

        string InputLanguage { get; }

        string TargetLanguage { get; }

        event TextRecognized Recognized;

        event TextRecognizing Recognizing;

        Task<string?> RecognizeOneAsync(CancellationToken cancellationToken);

        Task StartAsync();

        Task StopAsync();
    }
}
