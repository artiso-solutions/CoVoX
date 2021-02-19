using System;
using System.Linq;
using Serilog;

namespace API.Modules
{
    public class TextRecognizedArgs : EventArgs
    {
        public string Text { get; }

        public TextRecognizedArgs(string translatedText)
        {
            Text = new string(translatedText.Where(c => !char.IsPunctuation(c)).ToArray());
            Log.Debug($"Detected text: {translatedText}");
        }
    }
}
