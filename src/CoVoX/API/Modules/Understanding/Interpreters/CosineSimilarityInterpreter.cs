using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Covox.Understanding;
using F23.StringSimilarity;

namespace Covox.Modules.Understanding.Interpreters
{
    public class CosineSimilarityInterpreter : IInterpreter
    {
        public double CalculateMatchScore(string target, string input)
        {
            var cosine = new Cosine(2);
            var distance = cosine.Distance(input, target);
            return 1.0 - distance;
        }
    }
}
