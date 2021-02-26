using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covox.Modules.Understanding.Interpreters;
using Covox.Translating;
using Covox.Understanding;
using Covox.Utils;
using Microsoft.Extensions.Logging;

namespace Covox
{
    public class CovoxEngine
    {
        private readonly UnderstandingModule _understandingModule;
        private readonly RecognitionLoop _recognitionLoop;
        private readonly ILogger _logger;

        public CovoxEngine(Configuration configuration)
            : this(configuration, DefaultLogger.CreateLogger())
        {
        }

        public CovoxEngine(Configuration configuration, ILogger logger)
        {
            _logger = logger;

            var errors = ModelValidator.Validate(configuration);

            if (errors.Any())
                throw new AggregateException(errors.Select(error => new Exception(error.ErrorMessage)).ToList());

            var translationModule =
                new MultiLanguageTranslator(configuration.AzureConfiguration, configuration.InputLanguages);
            _understandingModule =
                new UnderstandingModule(new CosineSimilarityInterpreter(), configuration.MatchingThreshold);

            _recognitionLoop = new RecognitionLoop(translationModule, _understandingModule);
            _recognitionLoop.Recognized += (command, context) => Recognized?.Invoke(command, context);
        }

        public bool IsActive => _recognitionLoop.IsActive;

        public event CommandRecognized Recognized;

        public async Task StartAsync()
        {
            _logger.LogDebug("Starting recognition");
            await _recognitionLoop.StartAsync();
        }

        public async Task StopAsync()
        {
            _logger.LogDebug("Stopping recognition");
            await _recognitionLoop.StopAsync();
        }

        public void RegisterCommands(List<Command> commands) =>
            _understandingModule.RegisterCommands(commands);
    }
}
