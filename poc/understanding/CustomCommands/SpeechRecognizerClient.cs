using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CustomCommands
{
    internal class SpeechRecognizerClient
    {
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
            
            Console.WriteLine($"Start Listening for keyword {keywordName} with KeywordRecognizer...");
            
            var t = await keywordRecognizer.RecognizeOnceAsync(keywordModel);
                
            if (t.Reason == ResultReason.RecognizedKeyword)
            {
                Console.WriteLine($"KEYWORD RECOGNIZED (KeywordRecognizer.Recognized): {keywordName}");
            }
            else
            {
                Console.WriteLine("Return to listen for keywords...");
                
                await StartRecognitionWithKeywordRecognizer(keywordName, keywordModelFile);
            }
        }
    }
}
