using System.Threading;
using System.Threading.Tasks;

namespace API
{
    public delegate void TextRecognized(string text);

    public interface ITranslatingModule
    {
        bool IsActive { get; }

        string InputLanguage { get; }

        string TargetLanguage { get; }

        event TextRecognized Recognized;

        Task<string> RecognizeOneAsync(CancellationToken cancellationToken);

        Task StartAsync();

        Task StopAsync();
    }
}
