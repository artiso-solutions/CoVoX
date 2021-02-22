using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Modules;
using API.Modules.Understanding.Interpreters;
using API.Utils;
using Serilog;

namespace API
{
    public class Covox
    {
        private readonly UnderstandingModule _understandingModule;
        private readonly RecognitionLoop _recognitionLoop;

        public Covox(Configuration configuration)
        {
            var errors = ModelValidator.ValidateModel(configuration);

            if (errors.Any())
                throw new AggregateException(errors.Select(error => new Exception(error.ErrorMessage)).ToList());

            var translationModule = new MultiLanguageTranslator(configuration.AzureConfiguration, configuration.InputLanguages);
            _understandingModule = new UnderstandingModule(new SimilarityInterpreter(), configuration.MatchingThreshold);
            _recognitionLoop = new RecognitionLoop(translationModule, _understandingModule);
        }

        public event CommandRecognized Recognized;

        public async Task StartAsync()
        {
            await _recognitionLoop.StartAsync();
            Log.Debug("Started listening for commands");
        }

        public async Task StopAsync()
        {
            await _recognitionLoop.StopAsync();
            Log.Debug("Stopped listening for commands");
        }

        public void RegisterCommands(List<Command> commands) =>
            _understandingModule.RegisterCommands(commands);
    }
}
