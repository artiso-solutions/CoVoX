using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace API.Modules
{
    public class Translator : ITranslatingModule
    {
        public event EventHandler<TextRecognizedArgs> TextRecognized;

        private readonly TranslationRecognizer _translationRecognizer;

        public Translator(Configuration configuration)
        {
            var translationConfig = SpeechTranslationConfig.FromSubscription(configuration.AzureConfiguration.SubscriptionKey, configuration.AzureConfiguration.Region);
            translationConfig.AddTargetLanguage("en-US");
            translationConfig.SpeechRecognitionLanguage = configuration.InputLanguages.First();

            _translationRecognizer = new TranslationRecognizer(translationConfig);

            _translationRecognizer.Recognized += (_, args) =>
            {
                if (args.Result.Reason == ResultReason.TranslatedSpeech)
                {
                    var translatedText = args.Result.Translations.Values.FirstOrDefault();
                    TextRecognized?.Invoke(this, new TextRecognizedArgs(translatedText));
                }
            };
        }

        public async Task StartVoiceRecognition()
        {
            await _translationRecognizer.StartContinuousRecognitionAsync();
        }

        public async Task StopVoiceRecognition()
        {
            await _translationRecognizer.StopContinuousRecognitionAsync();
        }

        public class TextRecognizedArgs : EventArgs
        {
            public string Text { get; }

            public TextRecognizedArgs(string translatedText)
            {
                Text = new string(translatedText.Where(c => !char.IsPunctuation(c)).ToArray());
            }
        }
    }
}
