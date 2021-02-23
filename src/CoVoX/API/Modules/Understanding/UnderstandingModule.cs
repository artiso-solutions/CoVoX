using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Serilog;

namespace API.Understanding
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
            var candidates = new List<Command>();

            foreach (var command in Commands)
            {
                command.MatchScore = CalculateHighestSimilarity(input, command);
                candidates.Add(command);
            }

            var matches = candidates.OrderByDescending(x => x.MatchScore)
                .Select(c => new Match {Command = c, MatchScore = c.MatchScore}).ToImmutableList();

            var bestMatch = matches.FirstOrDefault(m => m.MatchScore >= MatchingThreshold);

            return (bestMatch, matches);
        }

        private double CalculateHighestSimilarity(string input, Command command)
        {
            var highestPercentage = 0.0;
            foreach (var trigger in command.VoiceTriggers)
            {
                var percentage = _interpreter.CalculateMatchScore(trigger, input);
                if (percentage > highestPercentage)
                {
                    highestPercentage = percentage;
                }
            }

            return highestPercentage * 0.01;
        }
    }
}
