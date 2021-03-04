using System;
using System.Threading.Tasks;
using KeywordRecognition.Settings;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace KeywordRecognition
{
    internal class SpeechRecognizerClient
    {
        private readonly SpeechRecognitionClientConfiguration _configuration;
        
        public SpeechRecognizerClient(SpeechRecognitionClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Starts a KeywordRecognitionSession using the <see cref="KeywordRecognizer"/> 
        /// Once KeywordRecognizer.RecognizeOnceAsync runs
        /// its Task wait (with no limit of time) until the given keyword is recognized
        /// </summary>
        public async Task StartRecognitionWithKeywordRecognizer(string keywordName, string keywordModelFile)
        {
            var keywordModel = KeywordRecognitionModel.FromFile(keywordModelFile);
            
            var config = AudioConfig.FromDefaultMicrophoneInput();
            var keywordRecognizer = new KeywordRecognizer(config);
            
            Console.WriteLine($"Listen for keyword {keywordName} with KeywordRecognizer...");
            
            while (true)
            {
                var t = await keywordRecognizer.RecognizeOnceAsync(keywordModel);
                
                if (t.Reason == ResultReason.RecognizedKeyword)
                {
                    Console.WriteLine($"KEYWORD RECOGNIZED (KeywordRecognizer.Recognized): {keywordName}");
                }
            }
        }
        
        /// <summary>
        /// Starts a KeywordRecognitionSession using the <see cref="SpeechRecognizer"/> 
        /// Once KeywordRecognizer.RecognizeOnceAsync runs
        /// its Task wait until the given keyword is recognized
        /// </summary>
        public async Task StartRecognitionWithSpeechRecognizer()
        {
            var keywordModel = KeywordRecognitionModel.FromFile("SpeechStudio/c8fd1fc8-3f5c-4cb8-a3b8-6ccc2ef3b0a3.table");
            var keywordName = "HEY_COVOX";
            
            Console.WriteLine($"Listen for keyword {keywordName} with SpeechRecognizer...");
            
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US"});

            var config = SpeechConfig.FromSubscription(_configuration.SubscriptionKey, _configuration.Region);

            var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);
            
            await recognizer.StartKeywordRecognitionAsync(keywordModel);
            
            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedKeyword)
                {
                    Console.WriteLine($"KEYWORD RECOGNIZED (SpeechRecognizer.Recognized): {keywordName}");
                }
            };

            recognizer.Canceled += (s, e) => { Console.WriteLine("Recognizing cancelled"); };

            recognizer.Recognizing += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedKeyword)
                {
                    Console.WriteLine($"KEYWORD RECOGNIZED (SpeechRecognizer.Recognizing): {keywordName}");
                }
            };
        }
    }
}
