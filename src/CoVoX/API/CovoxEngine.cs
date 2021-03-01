using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covox.Modules.Understanding.Interpreters;
using Covox.Translating;
using Covox.Understanding;
using Microsoft.Extensions.Logging;

namespace Covox
{
    public class CovoxEngine : IExposeErrors
    {
        private readonly MultiLanguageTranslator _translationModule;
        private readonly UnderstandingModule _understandingModule;
        private readonly RecognitionLoop _recognitionLoop;
        private readonly ILogger _logger;

        public CovoxEngine(Configuration configuration)
            : this(DefaultLogger.CreateLogger<CovoxEngine>(), configuration)
        {
        }

        public CovoxEngine(
            ILogger<CovoxEngine> logger,
            Configuration configuration)
        {
            _logger = logger;

            var errors = ModelValidator.Validate(configuration);
            if (errors.Any()) throw errors.AsException();

            _translationModule = new MultiLanguageTranslator(
                configuration.AzureConfiguration, configuration.InputLanguages);

            _translationModule.OnError += ex => OnTranslationError(ex);

            _understandingModule = new UnderstandingModule(
                new CosineSimilarityInterpreter(), configuration.MatchingThreshold);

            _recognitionLoop = new RecognitionLoop(_translationModule, _understandingModule);
            _recognitionLoop.Recognized += (command, context) => Recognized?.Invoke(command, context);
        }

        public bool IsActive => _recognitionLoop.IsActive;

        public IReadOnlyList<Command> Commands => _understandingModule.Commands;

        public event CommandRecognized Recognized;

        public event ErrorHandler OnError;

        private void OnTranslationError(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            OnError?.Invoke(ex);
        }

        public async Task StartAsync()
        {
            if (!Commands.Any())
                throw new InvalidOperationException("No commands were registered.");

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
