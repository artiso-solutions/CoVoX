using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Modules;
using Serilog;

namespace API
{
    public class Covox
    {
        public event EventHandler<CommandDetectedArgs> CommandDetected;
        private readonly IUnderstandingModule _understandingModule;

        public Covox(Configuration configuration)
        {
            var translator = new Translator(configuration);
            _understandingModule = new Interpreter(translator);
            _understandingModule.CommandRecognized += (_, args) =>
            {
                if (args.Command == null) return;
                CommandDetected?.Invoke(this, new CommandDetectedArgs(args.Command));
            };
        }

        public async Task StartListening()
        {
            await _understandingModule.StartCommandDetection();
            Log.Debug("Started listening for commands");
        }

        public async Task StopListening()
        {
            await _understandingModule.StopCommandDetection();
            Log.Debug("Stopped listening for commands");
        }

        public void RegisterCommands(List<Command> commands)
        {
            try
            {
                _understandingModule.RegisterCommands(commands);
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }

        public class CommandDetectedArgs : EventArgs
        {
            public Command Command { get; }

            public CommandDetectedArgs(Command command)
            {
                Command = command;
            }
        }
    }
}
