using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace API.Modules
{
    public class Interpreter : IUnderstandingModule
    {
        private readonly ITranslatingModule _translator;
        public event EventHandler<CommandRecognizedArgs> CommandRecognized;

        private static IReadOnlyList<Command> _commands = new List<Command>();

        public Interpreter(Configuration configuration)
        {
            _translator = new Translator(configuration);
            _translator.TextRecognized += (_, args) =>
            {
                CommandRecognized?.Invoke(this, new CommandRecognizedArgs(args.Text));
            };
        }

        public void RegisterCommands(List<Command> commands)
        {
            _commands = commands;
            Log.Debug($"Registered commands");
        }

        public IReadOnlyList<Command> GetRegisteredCommands()
        {
            return _commands;
        }

        public async Task StartCommandDetection()
        {
            if (GetRegisteredCommands().Any())
            {
                await _translator.StartVoiceRecognition();
            }
            else
            {
                throw new Exception("No commands registered");
            }
        }

        public async Task StopCommandDetection()
        {
            await _translator.StopVoiceRecognition();
        }

        public class CommandRecognizedArgs : EventArgs
        {
            public Command Command { get; }

            public CommandRecognizedArgs(string text)
            {
                //TODO replace string matching (Text Analytics, Language Understanding?)
                var command =
                    _commands.FirstOrDefault(x => x.VoiceTriggers.Any(y => text.ToLower().Contains(y.ToLower())));
                if (command != null)
                {
                    Command = command;
                    Log.Debug($"Detected command: {command?.Id}");
                }
                else
                {
                    Log.Debug($"No matching command found for '{text}'");
                }
            }
        }
    }
}