//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace helloworld
{
    class Program
    {
        public static async Task RecognizeSpeechAsync()
        {
            var subscriptionKey = "subscriptionKey";
            var yourserviceregion = "yourserviceregion";
            
            var config = SpeechConfig.FromSubscription(subscriptionKey, yourserviceregion);
            
            var recognizer = new SpeechRecognizer(config);
            
            await recognizer.StartContinuousRecognitionAsync();

            recognizer.Recognized += (s, e) =>
            {
                Console.WriteLine($"We recognized: {e.Result.Text}");
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine("Recognizing cancelled");
            };

            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine("Recognizing...");
            };
            
            recognizer.SpeechStartDetected += (s, e) =>
            {
                Console.WriteLine("SpeechStartDetected");
            };

            recognizer.SpeechEndDetected += (s, e) =>
            {
                Console.WriteLine("SpeechEndDetected");
            };

            await Task.Delay(TimeSpan.FromMinutes(1));

            // THERE'S ALSO
            //recognizer.StartKeywordRecognitionAsync()
            
        }

        static async Task Main()
        {
            await RecognizeSpeechAsync();
            Console.WriteLine("Please press <Return> to continue.");
            Console.ReadLine();
        }
    }
}
