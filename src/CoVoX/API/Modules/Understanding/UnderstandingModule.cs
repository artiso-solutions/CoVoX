using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Covox.Understanding
{
    internal class UnderstandingModule : IUnderstandingModule, IExposeErrors
    {
        private const double CloseMatchesThreshold = 0.05;
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
            var evaluations = new List<Command>();

            foreach (var command in Commands)
            {
                try
                {
                    command.MatchScore = CalculateHighestSimilarity(input, command);
                    evaluations.Add(command);
                }
                catch (Exception ex)
                {
                    OnUnderstandError(ex);
                }
            }

            try
            {
                var matches = evaluations.OrderByDescending(x => x.MatchScore)
                    .Where(x => x.MatchScore >= MatchingThreshold)
                    .Select(c => new Match { Command = c, MatchScore = c.MatchScore }).ToList();

                var bestMatch = matches.FirstOrDefault();

                if (bestMatch is null) return (default, Array.Empty<Match>());
                {
                    var closeMatches = matches.Where(x => x.MatchScore >= bestMatch.MatchScore - CloseMatchesThreshold).ToImmutableList();
                    return (bestMatch, closeMatches);
                }
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
            input = NormalizeString(input);

            var highestPercentage = 0.0;
            foreach (var trigger in command.VoiceTriggers)
            {
                var percentage = _interpreter.CalculateMatchScore(NormalizeString(trigger), input);
                if (percentage > highestPercentage)
                {
                    highestPercentage = percentage;
                }
            }

            return highestPercentage;
        }

        private string NormalizeString(string input)
        {
            return new string(input.Where(c => !char.IsPunctuation(c)).ToArray()).ToLowerInvariant();
        }
    }
}
