using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FuzzySharp;

namespace API.Modules.Understanding.Interpreters
{
    public class SimilarityInterpreter : IInterpreter
    {
        public Command InterpretCommand(IReadOnlyList<Command> commands, string text)
        {
            Debug.WriteLine(text);
            var commandSimilarities = new List<CommandSimilarity>();
            foreach (var command in commands)
            {
                var result = CalculateHighestSimilarity(text, command);
                commandSimilarities.Add(new CommandSimilarity() { Command = command, Similarity = result });
            }

            var commandSimilarity = commandSimilarities.OrderByDescending(x => x.Similarity).FirstOrDefault();
            if (commandSimilarity?.Similarity < 80)
            {
                return null;
            }

            return commandSimilarity?.Command;
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
            var targetTokens = voiceTrigger.Split('_').ToList();
            var inputTokens = new List<string>();
            var cleanedCommand = voiceTrigger.Replace('_', ' ').ToLower();

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
            var percentageStringSimilarity = FuzzyWeightedRatio(input.ToLower(), cleanedCommand); //10%
            var percentageTokenSortRatio = FuzzyTokenSortRatio(cleanedInputTokenString, cleanedCommand); //10%

            var totalPercentage = percentageAmountOfKeywords * 0.8 + percentageStringSimilarity * 0.1 + percentageTokenSortRatio * 0.1;
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

            return highestPercentage;
        }

        public class CommandSimilarity
        {
            public double Similarity { get; set; }

            public Command Command { get; set; }
        }
    }
}