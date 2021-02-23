using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class CosineSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            // 2 character sequence
            var cosine = new Cosine(2);
            var distance = cosine.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
