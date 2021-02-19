using System;
using System.Threading.Tasks;
using API.Modules;

namespace API
{
    public interface ITranslatingModule
    {
        event EventHandler<Translator.TextRecognizedArgs> TextRecognized;
        Task StartVoiceRecognition();
        Task StopVoiceRecognition();
    }
}
