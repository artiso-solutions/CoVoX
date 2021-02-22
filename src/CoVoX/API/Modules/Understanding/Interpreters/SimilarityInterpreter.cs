using System.Collections.Generic;
using System.Linq;
using FuzzySharp;

namespace API.Modules.Understanding.Interpreters
{
    internal class SimilarityInterpreter : IInterpreter
    {
        public IReadOnlyList<Match> InterpretCommand(
            IReadOnlyList<Command> commands,
            string input)
        {
            var candidates = new List<Command>();

            foreach (var command in commands)
            {
                command.MatchScore = CalculateHighestSimilarity(input, command);
                candidates.Add(command);
            }

            candidates = candidates.OrderByDescending(x => x.MatchScore).ToList();

            return candidates.Select(c => new Match { Command = c, MatchScore = c.MatchScore }).ToArray();
        }

        private static double FuzzyTokenSortRatio(string input, string target)
        {
            return Fuzz.TokenSortRatio(input, target);
        }

        private static double FuzzyWeightedRatio(string input, string target)
        {
            return Fuzz.WeightedRatio(input, target);
        }

        private static double CalculateSimilarity(string input, string voiceTrigger)
        {
            var targetTokens = voiceTrigger.Split(' ').ToList();
            var inputTokens = new List<string>();

            foreach (var token in targetTokens)
            {
                var lowerToken = token.ToLower();
                if (input.ToLower().Contains(lowerToken))
                {
                    inputTokens.Add(token);
                }
            }

            var cleanedInputTokenString = "";
            foreach (var inputToken in inputTokens)
            {
                cleanedInputTokenString += $"{inputToken.ToLower()} ";
            }

            var percentageAmountOfKeywords = (inputTokens.Count / targetTokens.Count) * 100; //80%
            var percentageStringSimilarity = FuzzyWeightedRatio(input.ToLower(), voiceTrigger); //10%
            var percentageTokenSortRatio = FuzzyTokenSortRatio(cleanedInputTokenString, voiceTrigger); //10%

            var totalPercentage = percentageAmountOfKeywords * 0.8 + percentageStringSimilarity * 0.1 +
                                  percentageTokenSortRatio * 0.1;
            return totalPercentage;
        }

        private static double CalculateHighestSimilarity(string input, Command command)
        {
            var highestPercentage = 0.0;
            foreach (var trigger in command.VoiceTriggers)
            {
                var percentage = CalculateSimilarity(input, trigger);
                if (percentage > highestPercentage)
                {
                    highestPercentage = percentage;
                }
            }

            return highestPercentage / 100.0;
        }
    }
}
