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
        public static string GetSimilarityLevenshtein(string input, string target)
        {

            var normalizedLevenshtein = new NormalizedLevenshtein();
            return normalizedLevenshtein.Distance(input, target).ToString();
            //Console.WriteLine(l.Distance("My string", "My $tring"));
            //Console.WriteLine(l.Distance("My string", "My $tring"));
            //Console.WriteLine(l.Distance("My string", "My $tring"));
        }

        public static string GetSimilarityNGram(string input, string target)
        {
            var ngram = new NGram(4);
            return ngram.Distance(input, target).ToString();
        }
    }
}
