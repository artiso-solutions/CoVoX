using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Modules.Understanding;

namespace API
{
    public class Covox
    {
        public event EventHandler<CommandDetectedArgs> CommandDetected;
        private readonly IUnderstandingModule _understandingModule;

        public Covox(Configuration configuration)
        {
            _understandingModule = new Interpreter(configuration);
            _understandingModule.CommandRecognized += (_, args) =>
            {
                if (args.Command == null) return;
                CommandDetected?.Invoke(this, new CommandDetectedArgs(args.Command));
            };
        }

        public async Task StartListening()
        {
            await _understandingModule.StartCommandDetection();
        }

        public async Task StopListening()
        {
            await _understandingModule.StopCommandDetection();
        }

        public void RegisterCommands(List<Command> commands)
        {
            _understandingModule.RegisterCommands(commands);
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
