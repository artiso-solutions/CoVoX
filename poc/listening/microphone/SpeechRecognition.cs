using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using TextTranslate;

namespace SpeechToTextFromMic
{
    public class SpeechRecognition
    {
        public static SpeechRecognizer Recognizer { get; set; }
        private readonly string _subscriptionKey;
        private readonly string _region;
        private readonly Dictionary<string, SpeechRecognizer> _recognizers = new();
        private readonly Dictionary<string, Stopwatch> _stopwatches = new();

        public SpeechRecognition(string subscriptionKey, string region, List<string> languageConfigs)
        {
            _subscriptionKey = subscriptionKey;
            _region = region;
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);

            foreach (var languageConfig in languageConfigs)
            {
                var instanceCreation = Stopwatch.StartNew();
                Console.WriteLine($"SpeechRecognizer instance creation : {instanceCreation.ElapsedMilliseconds} ms");

                var recognizer = new SpeechRecognizer(config, AutoDetectSourceLanguageConfig.FromLanguages(new[] { languageConfig }));
                var stopWatch = new Stopwatch();

                _recognizers.Add(languageConfig, recognizer);
                _stopwatches.Add(languageConfig, stopWatch);

                recognizer.SessionStarted += (_, _) =>
                {
                    // Console.WriteLine($"SessionStarted ({languageConfig}): {stopWatch.ElapsedMilliseconds} ms");
                };

                recognizer.SpeechStartDetected += (_, _) =>
                {
                    stopWatch.Start();
                    // Console.WriteLine($"SpeechStartDetected ({languageConfig}): {stopWatch.ElapsedMilliseconds} ms");
                };

                recognizer.Recognizing += (_, _) =>
                {
                    // TODO : Could be published the recognizing
                    // Console.WriteLine($"Recognizing ({languageConfig}) ..... : {stopWatch.ElapsedMilliseconds} ms");
                    stopWatch.Restart();
                };

                Console.WriteLine($"SpeechRecognizer instance creation ({languageConfig}): {instanceCreation.ElapsedMilliseconds} ms");
                instanceCreation.Stop();
            }

        }

        public async Task RecognizeSpeechContinuous(string language)
        {
            var recognizer = _recognizers[language];
            var stopWatch = _stopwatches[language];

            await recognizer.StartContinuousRecognitionAsync();

            recognizer.Recognized += (sender, args) =>
            {
                var translationResult = TextTranslation.TranslateToEn(_subscriptionKey, _region, args.Result.Text).GetAwaiter().GetResult();
                var translation = translationResult.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(args.Result.Text))
                {
                    Console.WriteLine($"{language} - Detected: {args.Result.Text} - Translated: {translation?.Translations.FirstOrDefault()?.Text} ({translation?.DetectedLanguage.Language}) ({stopWatch.ElapsedMilliseconds} ms)");
                }
                else
                {
                    Console.Write(".");
                }

                stopWatch.Restart();
            };
        }

        public static async Task<(string Text, string DetectedLanguage)> RecognizeSpeech(string subscriptionKey, string region)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromOpenRange();
            var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.Canceled)
                throw new Exception("Api Communication Failure");

            var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;
            return (result.Text, detectedLanguage);
        }

        public static async Task<(string Text, string DetectedLanguage)> RecognizeSpeech(string subscriptionKey, string region, AutoDetectSourceLanguageConfig autoDetectSourceLanguageConfig)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.Canceled)
                throw new Exception("Api Communication Failure");

            var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;
            return (result.Text, detectedLanguage);
        }

        public static async Task RecognizeSpeechContinuous(string subscriptionKey, string region, AutoDetectSourceLanguageConfig autoDetectSourceLanguageConfig)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);

            Recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);

            Recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    var textResult = e.Result.Text;

                    var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);

                    var fromLanguage = autoDetectSourceLanguageResult.Language;

                    Console.WriteLine($"Recognized '{fromLanguage}': {textResult}");
                }
            };

            Recognizer.Canceled += (s, e) => { Console.WriteLine("Recognizing cancelled"); };

            Recognizer.Recognizing += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizingSpeech) Console.WriteLine("Recognizing...");
            };

            Recognizer.SpeechStartDetected += (s, e) => { Console.WriteLine("SpeechStartDetected"); };

            Recognizer.SpeechEndDetected += (s, e) => { Console.WriteLine("SpeechEndDetected"); };
         
            await Recognizer.StartContinuousRecognitionAsync();
        }
    }
}
