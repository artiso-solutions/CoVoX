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
            //Return from 0 - 100, is case dependent 
            target = target.ToLower();
            input = input.ToLower();
            double percentage = Fuzz.WeightedRatio(input, target);
            return percentage / 100;
        }
    }
}
