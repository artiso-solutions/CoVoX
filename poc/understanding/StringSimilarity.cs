using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F23.StringSimilarity;
using FuzzySharp;

namespace LanguageUnderstanding
{
    public static class StringSimilarity
    {
        public static string GetSimilarityNormalizedLevenshtein(string input, string target)
        {
            var normalizedLevenshtein = new NormalizedLevenshtein();
            return normalizedLevenshtein.Distance(input, target).ToString();
        }

        public static string GetSimilarityNGram(string input, string target)
        {
            var ngram = new NGram(4);
            return ngram.Distance(input, target).ToString();
        }

        public static string GetSimilarityLevenshtein(string input, string target)
        {
            var levenshtein = new F23.StringSimilarity.Levenshtein();
            return levenshtein.Distance(input, target).ToString();
        }

        public static string GetSimilarityDamerau(string input, string target)
        {
            var damerau = new Damerau();
            return damerau.Distance(input, target).ToString();
        }

        public static string GetSimilarityJaroWinkler(string input, string target)
        {
            var jaroWinkler = new JaroWinkler();
            return jaroWinkler.Distance(input, target).ToString();
        }

        public static string GetSimilarityLongestCommonSubsequence(string input, string target)
        {
            var lcs = new LongestCommonSubsequence();
            return lcs.Distance(input, target).ToString();
        }

        public static string GetSimilarityMetricLongestCommonSubsequence(string input, string target)
        {
            var mlcs = new MetricLCS();
            return mlcs.Distance(input, target).ToString();
        }

        public static void CombineMethods(string input, string target)
        {
            //Levenshtein
            Console.WriteLine("Levenshtein:");
            Console.WriteLine($"{GetSimilarityLevenshtein(input, target)}\n");

            //normalized Levenshtein
            Console.WriteLine("Normalized Levenshtein");
            Console.WriteLine($"{GetSimilarityNormalizedLevenshtein(input, target)}\n");

            //N-Gram
            Console.WriteLine("N-Gram");
            Console.WriteLine($"{GetSimilarityNGram(input, target)}\n");

            //Damerau
            Console.WriteLine("Damerau");
            Console.WriteLine($"{GetSimilarityDamerau(input, target)}\n");

            //Jaro Winkler
            Console.WriteLine("Jaro Winkler");
            Console.WriteLine($"{GetSimilarityJaroWinkler(input, target)}\n");

            //Longest Common Subsequence
            Console.WriteLine("Longest Common Subsequence");
            Console.WriteLine($"{GetSimilarityLongestCommonSubsequence(input, target)}\n");

            //Metric Longest Common Subsequence
            Console.WriteLine("Metric Longest Common Subsequence");
            Console.WriteLine($"{GetSimilarityMetricLongestCommonSubsequence(input, target)}\n");

            // Fuzzy Sort
            Console.WriteLine("Metric Longest Common Subsequence");
            Console.WriteLine($"{FuzzyTokenSortRatio(input, target)}\n");

            // Fuzzy Weight
            Console.WriteLine("Metric Longest Common Subsequence");
            Console.WriteLine($"{GetSimilarityMetricLongestCommonSubsequence(input, target)}\n");
        }

        public static double FuzzyTokenSortRatio(string input, string target)
        {
            return Fuzz.TokenSortRatio(input, target);
        }

        // get % of similarity
        public static double FuzzyWeightedRatio(string input, string target)
        {
            return Fuzz.WeightedRatio(input, target);
        }

        public static void CalculateSimilarity(string input, string target)
        {
            var targetTokens = target.Split('_').ToList();
            var inputTokens = new List<string>();
            var cleanedCommand = target.Replace('_', ' ').ToLower();

            foreach (var token in targetTokens)
            {
                var lowertoken = token.ToLower();
                if(input.ToLower().Contains(lowertoken))
                {
                    inputTokens.Add(token);
                }
            }

            var cleanedInputTokenString = "";
            foreach (string inputToken in inputTokens)
            {
                cleanedInputTokenString += $"{inputToken.ToLower()} ";
            }

            var percentageAmountOfKeywords = (inputTokens.Count / targetTokens.Count) * 100; //80%
            var percentageStringSimilarity = FuzzyWeightedRatio(input.ToLower(), cleanedCommand); //10%
            var percentageTokenSortRatio = FuzzyTokenSortRatio(cleanedInputTokenString, cleanedCommand); //10%

            var totalPercentage = percentageAmountOfKeywords * 0.8 + percentageStringSimilarity * 0.1 + percentageTokenSortRatio * 0.1;

            Console.WriteLine($"percentage of keywords {percentageAmountOfKeywords} percentage string similarity {percentageStringSimilarity} percentage token sort ratio {percentageTokenSortRatio}" );
            Console.WriteLine($"total similarity in % {totalPercentage}");
        }
    }
}
