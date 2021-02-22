using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FuzzySharp;

namespace API.Modules.Understanding.Interpreters
{
    public class SimilarityInterpreter : IInterpreter
    {
        public (Command, IReadOnlyList<Command>) InterpretCommand(IReadOnlyList<Command> commands, double matchingThreshold,
            string text)
        {
            Debug.WriteLine(text);
            var candidates = new List<Command>();
            foreach (var command in commands)
            {
                command.MatchScore = CalculateHighestSimilarity(text, command);
                candidates.Add(command);
            }

            candidates = candidates.OrderByDescending(x => x.MatchScore).ToList();

            var matchedCommand = candidates.FirstOrDefault()?.MatchScore >= matchingThreshold
                ? candidates.FirstOrDefault()
                : null;

            return (matchedCommand, candidates);
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
