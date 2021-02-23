using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class DamerauSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var damerau = new Damerau();
            var distance = damerau.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
