using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace API.Modules
{
    internal class UnderstandingModule : IUnderstandingModule
    {
        private readonly IInterpreter _interpreter;

        public UnderstandingModule(
            IInterpreter interpreter,
            double matchingThreshold)
        {
            _interpreter = interpreter;
            MatchingThreshold = matchingThreshold;
        }

        public IReadOnlyList<Command> Commands { get; private set; } = Array.Empty<Command>();

        public double MatchingThreshold { get; }

        public void RegisterCommands(IEnumerable<Command> commands)
        {
            Commands = commands?.ToList();
            Log.Debug("Registered commands");
        }

        public (Match bestMatch, IReadOnlyList<Match> candidates) Understand(string input)
        {
            var matches = _interpreter.InterpretCommand(Commands, input);

            var bestMatch = matches
                .Where(m => m.MatchScore >= MatchingThreshold)
                .OrderByDescending(m => m.MatchScore)
                .FirstOrDefault();

            return (bestMatch, matches);
        }
    }
}
