using System;
using System.Threading.Tasks;
using CustomCommands.Configuration;
using CustomCommands.Events;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Dialog;

namespace CustomCommands
{
    class CustomCommandsClient
    {
        private readonly BaseEventHandlers _baseEventHandlers;
        private DialogServiceConnector _recognizer;
        private string _user;

        public CustomCommandsClient(
            CustomCommandClientConfiguration configuration, 
            AudioConfig audioConfig,
            BaseEventHandlers baseEventHandlers)
        {
            _baseEventHandlers = baseEventHandlers;

            CreateRecognizer(configuration, audioConfig);
        }

        private void CreateRecognizer(CustomCommandClientConfiguration configuration, AudioConfig audioConfig)
        {
            var customCommandsConfig = CustomCommandsConfig.FromSubscription(
                configuration.AppId, 
                configuration.SubscriptionKey, 
                configuration.Region);
            
            _recognizer = new DialogServiceConnector(customCommandsConfig, audioConfig);

            _recognizer.TurnStatusReceived += _baseEventHandlers.OnTurnStatusReceivedHandler;
            _recognizer.Recognized += _baseEventHandlers.OnRecognizedHandler;
            _recognizer.Recognizing += _baseEventHandlers.OnRecognizingHandler;
            _recognizer.ActivityReceived += _baseEventHandlers.OnActivityReceivedHandler;
            _recognizer.Canceled += _baseEventHandlers.OnCanceledHandler;
        }

        public async Task Connect()
        {
            Console.WriteLine("connecting..");
            
            await _recognizer.ConnectAsync();
        }
        
        public async Task StartListenForCommands()
        {
            Console.WriteLine("Start listening for commands: ");
            
            await _recognizer.ListenOnceAsync();
        }
    }
}
