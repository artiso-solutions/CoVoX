using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class JaroWinklerSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var jaroWinkler = new JaroWinkler();
            var distance = jaroWinkler.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
