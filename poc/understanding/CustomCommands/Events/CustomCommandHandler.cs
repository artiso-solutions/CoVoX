using System;
using System.Text.Json;
using System.Threading.Tasks;
using CustomCommands.Configuration;
using CustomCommands.Messages;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Dialog;

namespace CustomCommands.Events
{
    internal class CustomCommandHandler : BaseEventHandlers
    {
        private readonly SpeechSynthesizer _synthesizer;
        public readonly CustomCommandsClient Client;
        public bool ConversationInProgress = false;

        public CustomCommandHandler(CustomCommandClientConfiguration configuration, SpeechSynthesizer synthesizer)
        {
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            
            var customCommandClient = new CustomCommandsClient(configuration, audioConfig);
            
            Client = customCommandClient;
            _synthesizer = synthesizer;
            
            customCommandClient.Recognizer.TurnStatusReceived += OnTurnStatusReceivedHandler;
            customCommandClient.Recognizer.Recognized += OnRecognizedHandler;
            customCommandClient.Recognizer.Recognizing += OnRecognizingHandler;
            customCommandClient.Recognizer.ActivityReceived += OnActivityReceivedHandler;
            customCommandClient.Recognizer.Canceled += OnCanceledHandler;
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

            if (result != null && result.Type == "CUSTOM.COMMAND")
            {
                Console.WriteLine("command detected");
            
                await _synthesizer.SpeakTextAsync($"command {result.Name} detected");

                return;
            }

            if (e.HasAudio)
            {
                await TextToSpeechResponse(e, options);
            }

            if (result?.InputHint == "expectingInput")
            {
                await Task.Delay(50);
                
                // Utterance limited listening for commands
                await Client.StartListenForInputs();
            }
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
