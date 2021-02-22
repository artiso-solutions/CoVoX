using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Modules;
using API.Modules.Understanding.Interpreters;
using Serilog;
using API.Utils;

namespace API
{
    public class Covox
    {
        public event EventHandler<CommandDetectedArgs> CommandDetected;
        private readonly IUnderstandingModule _understandingModule;

        public Covox(Configuration configuration)
        {
            var errors = ModelValidator.ValidateModel(configuration);

            if (!errors.Any())
            {
                var translator = new Translator(configuration);
                var similarityInterpreter = new SimilarityInterpreter();
                _understandingModule = new UnderstandingModule(translator, similarityInterpreter, configuration.MatchingThreshold);
                _understandingModule.CommandRecognized += (_, args) =>
                {
                    if (args.Command == null) return;
                    CommandDetected?.Invoke(this, new CommandDetectedArgs(args.Command, args.DetectionContext));
                };
            }
            else
            {
                throw new AggregateException(errors.Select(error => new Exception(error.ErrorMessage)).ToList());
            }
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
            public DetectionContext DetectionContext { get; }

            public CommandDetectedArgs(Command command, DetectionContext context)
            {
                Command = command;
                DetectionContext = context;
            }
        }
    }
}
