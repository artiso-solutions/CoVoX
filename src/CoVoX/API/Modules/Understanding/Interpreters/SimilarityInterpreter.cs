using System.Collections.Generic;
using System.Linq;
using FuzzySharp;

namespace API.Understanding
{
    internal class SimilarityInterpreter : IInterpreter
    {
        public double CalculateMatchScore(string target, string input)
        {
            var targetTokens = target.Split(' ').ToList();
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

            var percentageAmountOfKeywords = (inputTokens.Count / targetTokens.Count) * 100;
            var percentageStringSimilarity = FuzzyWeightedRatio(input.ToLower(), target);
            var percentageTokenSortRatio = FuzzyTokenSortRatio(cleanedInputTokenString, target);

            return percentageAmountOfKeywords * 0.8 +
                   percentageStringSimilarity * 0.1 +
                   percentageTokenSortRatio * 0.1;
        }

        private static double FuzzyTokenSortRatio(string input, string target)
        {
            return Fuzz.TokenSortRatio(input, target);
        }

        private static double FuzzyWeightedRatio(string input, string target)
        {
            return Fuzz.WeightedRatio(input, target);
        }
    }
}
