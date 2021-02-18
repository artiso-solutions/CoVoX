using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F23.StringSimilarity;

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

            var levenshtein = new Levenshtein();
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

        }
    }
}
