using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class LevenshteinSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var levenshtein = new F23.StringSimilarity.Levenshtein();
            var distance = levenshtein.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
