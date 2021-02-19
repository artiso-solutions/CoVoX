using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using Serilog;

namespace API.Modules
{
    public class Translator : ITranslatingModule
    {
        public event EventHandler<TextRecognizedArgs> TextRecognized;

        private readonly List<TranslationRecognizer> _translationRecognizers = new();

        public Translator(Configuration configuration)
        {
            try
            {
                foreach (var inputLanguage in configuration.InputLanguages)
                {
                    var translationConfig = SpeechTranslationConfig.FromSubscription(
                        configuration.AzureConfiguration.SubscriptionKey, configuration.AzureConfiguration.Region);
                    translationConfig.AddTargetLanguage("en-US");
                    translationConfig.SetProfanity(ProfanityOption.Raw);
                    translationConfig.SpeechRecognitionLanguage = inputLanguage;

                    var translationRecognizer = new TranslationRecognizer(translationConfig);

                    translationRecognizer.Recognized += (_, args) =>
                    {
                        if (args.Result.Reason == ResultReason.TranslatedSpeech)
                        {
                            var translatedText = args.Result.Translations.Values.FirstOrDefault();
                            TextRecognized?.Invoke(this, new TextRecognizedArgs(translatedText));
                        }
                    };

                    _translationRecognizers.Add(translationRecognizer);
                }
            }
            catch
            {
                throw new Exception("Error initializing TranslationRecognizer. Missing Azure subscription key?");
            }
        }

        public async Task StartVoiceRecognition()
        {
            foreach (var translationRecognizer in _translationRecognizers)
            {
                await translationRecognizer.StartContinuousRecognitionAsync();
                Log.Debug($"Started continuous recognition {translationRecognizer.SpeechRecognitionLanguage}");
            }
        }

        public async Task StopVoiceRecognition()
        {
            foreach (var translationRecognizer in _translationRecognizers)
            {
                await translationRecognizer.StopContinuousRecognitionAsync();
                Log.Debug($"Stopped continuous recognition {translationRecognizer.SpeechRecognitionLanguage}");
            }
        }
    }
}
