﻿using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToTextFromInMemoryStream
{
    public class SpeechRecognition
    {
        public static async Task<(string Text, string DetectedLanguage)> RecognizeSpeechFromStream(string subscriptionKey, string region, AudioInputStream audioInputStream, AutoDetectSourceLanguageConfig autoDetectSourceLanguageConfig)
        {
            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
            using var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
            using var recognizer = new SpeechRecognizer(speechConfig, autoDetectSourceLanguageConfig, audioConfig);

            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.Canceled)
                throw new Exception("Api Communication Failure");

            var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;
            return (result.Text, detectedLanguage);
        }
    }
}
