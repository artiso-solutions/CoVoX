using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Covox.Understanding
{
    internal class UnderstandingModule : IUnderstandingModule
    {
        private readonly IInterpreter _interpreter;

        internal UnderstandingModule(
            IInterpreter interpreter,
            double matchingThreshold)
        {
            _interpreter = interpreter;
            MatchingThreshold = matchingThreshold;
        }

        public IReadOnlyList<Command> Commands { get; private set; } = Array.Empty<Command>();

        private double MatchingThreshold { get; }

        public void RegisterCommands(IEnumerable<Command> commands)
        {
            Commands = commands?.ToList();
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
