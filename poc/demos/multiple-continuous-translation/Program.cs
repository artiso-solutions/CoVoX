using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using TextTranslate;

namespace MultipleContinuousTranslation
{
    class Program
    {
        private static readonly string SpeechSubscriptionKey = "SpeechSubscriptionKey";
        private static readonly string TranslatorKey = "TranslatorKey";
        private static readonly string Region = "Region";

        static async Task Main(string[] args)
        {
            var taskEn = TranslateSpeechContinuous(SpeechSubscriptionKey, TranslatorKey, Region, "en-US");
            var taskDe = TranslateSpeechContinuous(SpeechSubscriptionKey, TranslatorKey, Region, "de-DE");
            var taskIt = TranslateSpeechContinuous(SpeechSubscriptionKey, TranslatorKey, Region, "it-IT");
            var taskEs = TranslateSpeechContinuous(SpeechSubscriptionKey, TranslatorKey, Region, "es-ES");
            
            await Task.WhenAll(taskEn, taskDe, taskIt, taskEs);
            
            Console.ReadKey();
        }

        private static async Task TranslateSpeechContinuous(string subscriptionKey, string translatorKey, string region, string language)
        {
            try
            {
                var startTime = DateTime.Now;

                var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { language });

                var config = SpeechConfig.FromSubscription(subscriptionKey, region);

                var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);

                recognizer.Recognizing += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizingSpeech) Console.WriteLine($"{language};Recognizing...;{e.Result.Text};{(DateTime.Now - startTime).TotalMilliseconds} ms");
                };

                recognizer.Recognized += async (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        var text = e.Result.Text;

                        var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);

                        Console.WriteLine($"{language};Recognized;{text};{(DateTime.Now - startTime).TotalMilliseconds} ms");

                        var detectedLanguage = autoDetectSourceLanguageResult.Language;

                        if (text != "" && language.Contains(detectedLanguage))
                        {
                            var translationResult = await TextTranslation.TranslateToEn(translatorKey, region, text);
                            var translation = translationResult.FirstOrDefault();

                            Console.WriteLine($"{language};Translated;{translation?.Translations.FirstOrDefault()?.Text} ({translation?.DetectedLanguage.Language});{(DateTime.Now - startTime).TotalMilliseconds} ms");
                        }
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"{language};Recognizing cancelled; ;{(DateTime.Now - startTime).TotalMilliseconds} ms");
                };

                recognizer.SpeechStartDetected += (s, e) =>
                {
                    Console.WriteLine($"{language};SpeechStartDetected; ;{(DateTime.Now - startTime).TotalMilliseconds} ms"); 
                };

                recognizer.SpeechEndDetected += (s, e) =>
                {
                    Console.WriteLine($"{language};SpeechEndDetected; ;{(DateTime.Now - startTime).TotalMilliseconds} ms"); 
                };

                await recognizer.StartContinuousRecognitionAsync();

                Console.WriteLine($"{language};Listening...; ;{(DateTime.Now - startTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
