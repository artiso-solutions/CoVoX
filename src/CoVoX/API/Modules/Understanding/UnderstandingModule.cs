using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace API.Modules
{
    public class UnderstandingModule : IUnderstandingModule
    {
        private readonly ITranslatingModule _translator;
        public event EventHandler<CommandRecognizedArgs> CommandRecognized;

        private static IReadOnlyList<Command> _commands = new List<Command>();

        public UnderstandingModule(ITranslatingModule translator, IInterpreter interpreter)
        {
            _translator = translator;
            _translator.TextRecognized += (_, args) =>
            {
                CommandRecognized?.Invoke(this, new CommandRecognizedArgs(interpreter, _commands, args.Text));
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
    }
}
