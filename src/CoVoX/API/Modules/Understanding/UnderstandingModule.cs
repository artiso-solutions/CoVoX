using System;
using System.Collections.Generic;
using System.Linq;

namespace Covox.Understanding
{
    internal class UnderstandingModule : IUnderstandingModule, IExposeErrors
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

        public event ErrorHandler OnError;

        public void RegisterCommands(IEnumerable<Command> commands)
        {
            Commands = commands?.ToList();
        }

        public (Match bestMatch, IReadOnlyList<Match> candidates) Understand(string input)
        {
            var candidates = new List<Command>();

            foreach (var command in Commands)
            {
                try
                {
                    command.MatchScore = CalculateHighestSimilarity(input, command);
                    candidates.Add(command);
                }
                catch (Exception ex)
                {
                    OnUnderstandError(ex);
                }
            }

            try
            {
                var matches = candidates.OrderByDescending(x => x.MatchScore)
                    .Where(x => x.MatchScore >= MatchingThreshold - 0.05)
                    .Select(c => new Match { Command = c, MatchScore = c.MatchScore }).ToList();

                var bestMatch = matches.FirstOrDefault(m => m.MatchScore >= MatchingThreshold);

                return (bestMatch, matches);
            }
            catch (Exception ex)
            {
                OnUnderstandError(ex);
                return (default, Array.Empty<Match>());
            }
        }

        private void OnUnderstandError(Exception ex)
        {
            OnError?.Invoke(ex);
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

            return highestPercentage;
        }
    }
}
