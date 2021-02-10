using System;
using System.Threading.Tasks;
using KeywordRecognition.Settings;
using Microsoft.CognitiveServices.Speech;

namespace KeywordRecognition
{
    internal class SpeechRecognizerClient
    {
        private readonly SpeechRecognitionClientConfiguration _configuration;

        public SpeechRecognizerClient(SpeechRecognitionClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task StartKeyWordRecognition()
        {
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US"});

            var config = SpeechConfig.FromSubscription(_configuration.SubscriptionKey, _configuration.Region);

            var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);

            var keyRecognitionModel = KeywordRecognitionModel.FromFile("SpeechStudio/c8fd1fc8-3f5c-4cb8-a3b8-6ccc2ef3b0a3.table");
            
            Console.WriteLine("Start listening");
            
            await recognizer.StartKeywordRecognitionAsync(keyRecognitionModel);
            
            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedKeyword)
                {
                    var textResult = e.Result.Text;
                    Console.WriteLine($"KEYWORD RECOGNIZED : HEY COVOX -> {textResult}");
                }
            };

            recognizer.Canceled += (s, e) => { Console.WriteLine("Recognizing cancelled"); };

            recognizer.Recognizing += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizingSpeech) Console.WriteLine("Recognizing...");
            };

            recognizer.SpeechStartDetected += (s, e) => { Console.WriteLine("SpeechStartDetected"); };

            recognizer.SpeechEndDetected += (s, e) => { Console.WriteLine("SpeechEndDetected"); };
        }
    }
}
