using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using Serilog;

namespace API.Modules
{
    public class Translator : ITranslatingModule
    {
        public event EventHandler<TextRecognizedArgs> TextRecognized;

        private readonly TranslationRecognizer _translationRecognizer;

        public Translator(Configuration configuration)
        {
            try
            {
                var translationConfig = SpeechTranslationConfig.FromSubscription(configuration.AzureConfiguration.SubscriptionKey, configuration.AzureConfiguration.Region);
                translationConfig.AddTargetLanguage("en-US");
                translationConfig.SpeechRecognitionLanguage = configuration.InputLanguages.First();
                translationConfig.SetProfanity(ProfanityOption.Raw);

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
            catch
            {
                throw new Exception("Error initializing TranslationRecognizer. Missing Azure subscription key?");
            }
        }

        public async Task StartVoiceRecognition()
        {
            await _translationRecognizer.StartContinuousRecognitionAsync();
            Log.Debug("Started continuous recognition");
        }

        public async Task StopVoiceRecognition()
        {
            await _translationRecognizer.StopContinuousRecognitionAsync();
            Log.Debug("Stopped continuous recognition");
        }

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
}
