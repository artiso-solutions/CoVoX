using System;
using System.Text.Json;
using System.Threading.Tasks;
using CustomCommands.Messages;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Dialog;

namespace CustomCommands.Events
{
    internal class CustomBaseEventHandlers : BaseEventHandlers
    {
        private SpeechSynthesizer _synthesizer;
        
        public CustomBaseEventHandlers(SpeechSynthesizer synthesizer)
        {
            _synthesizer = synthesizer;
        }
        
        public override void OnTurnStatusReceivedHandler(object sender, TurnStatusReceivedEventArgs e)
        {
            //
        }
        
        public override void OnCanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e)
        {
            //
        }
        
        public override void OnRecognizingHandler(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine(".");
        }

        public override void OnRecognizedHandler(object sender, SpeechRecognitionEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Result.Text)) 
                Console.WriteLine($"YOU -> {e.Result.Text}");
        }

        public override async void OnActivityReceivedHandler(object sender, ActivityReceivedEventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<ActivityEvent>(e.Activity, options);

            if (result?.Name == "MYEVENT")
            {
                Console.WriteLine("command detected");
                await _synthesizer.SpeakTextAsync("command detected");
            }

            if (e.HasAudio) await TextToSpeechResponse(e, options);
        }

        private async Task TextToSpeechResponse(ActivityReceivedEventArgs e, JsonSerializerOptions options)
        {
            var message = JsonSerializer.Deserialize<StandardMessageEvent>(e.Activity, options);

            if (message is not null)
            {
                Console.WriteLine($"SERVICE -> {message.Text}");

                await _synthesizer.SpeakTextAsync(message.Text);
            }
        }
    }
}
