using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;

namespace LanguageUnderstanding.SimilarityCalculators 
{
    public class FuzzyWeightedRatioCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            double percentage = Fuzz.WeightedRatio(input, target);
            return percentage / 100;
        }
    }
}
