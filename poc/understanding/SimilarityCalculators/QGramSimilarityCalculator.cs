using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    class QGramSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var dig = new QGram(2);
            var distance = dig.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
