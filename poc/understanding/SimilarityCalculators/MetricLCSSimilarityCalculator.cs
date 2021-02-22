using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageUnderstanding.SimilarityCalculators
{
    public class MetricLCSSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var mlcs = new MetricLCS();
            var distance = mlcs.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
