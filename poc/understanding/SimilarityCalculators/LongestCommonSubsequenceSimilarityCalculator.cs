using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class LongestCommonSubsequenceSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var lcs = new LongestCommonSubsequence();
            var distance = lcs.Distance(input, target);
            // TODO
            return 1.0 - distance;
        }
    }
}
