using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class NGramSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var ngram = new NGram(4);
            var distance = ngram.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
