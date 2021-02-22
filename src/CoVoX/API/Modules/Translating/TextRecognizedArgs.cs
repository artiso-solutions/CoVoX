using System;
using System.Linq;
using Serilog;

namespace API.Modules
{
    public class TextRecognizedArgs : EventArgs
    {
        public string Text { get; }
        public string InputLanguage { get; set; }
        public TextRecognizedArgs(string translatedText, string inputLanguage)
        {
            Text = new string(translatedText.Where(c => !char.IsPunctuation(c)).ToArray());
            InputLanguage = inputLanguage;
            Log.Debug($"Detected text ('{inputLanguage}'): {translatedText}");
        }
    }
}
