using System.Threading.Tasks;
using LanguageUnderstanding.SimilarityCalculators;

namespace LanguageUnderstanding
{
    class Program
    {
        static async Task Main()
        {
            // await Demo.Run();

            /*
             * Let's have many implementations of ISimilarityCalculator, using both
             * StringSimilarity, FuzzySharp and our custom token matching algorithm.
             * Test all the implementations, in order to be compliant with expectations.
             * 
             * StringSimilarity algorithms: https://github.com/feature23/StringSimilarity.NET#overview
             * FuzzySharp algorithms: https://github.com/JakeBayer/FuzzySharp#usage
             */
            
            Test.With(new FuzzyWeightedRatioCalculator()); //not case sensitive
            Test.With(new CosineSimilarityCalculator()); //case sensitive
            Test.With(new DamerauSimilarityCalculator()); //case sensitive
            Test.With(new JaroWinklerSimilarityCalculator()); //case sensitive
            Test.With(new LevenshteinSimilarityCalculator()); // case sensitive
            Test.With(new LongestCommonSubsequenceSimilarityCalculator()); //case sensitive
            Test.With(new MetricLCSSimilarityCalculator()); //case sensitive
            Test.With(new NGramSimilarityCalculator()); //case sensitive
            Test.With(new NormalizedLevenshteinSimilarityCalculator()); //case sensitive
            Test.With(new QGramSimilarityCalculator()); // case sensitive
        }
    }
}
