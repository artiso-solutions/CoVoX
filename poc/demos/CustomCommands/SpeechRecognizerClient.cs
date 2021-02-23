using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CustomCommands
{
    internal class SpeechRecognizerClient
    {
        private readonly SpeechSynthesizer _synthesizer;

        public SpeechRecognizerClient(SpeechSynthesizer synthesizer)
        {
            _synthesizer = synthesizer;
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
            
            var t = await keywordRecognizer.RecognizeOnceAsync(keywordModel);
                
            if (t.Reason == ResultReason.RecognizedKeyword)
            {
                await _synthesizer.SpeakTextAsync("I'm here");
            }
        }
    }
}
