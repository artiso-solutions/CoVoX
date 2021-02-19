using System.Threading.Tasks;

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
            
            // Test.With(new DummySimilarityCalculator());
            Test.With(new NormalizedLevenshteinSimilarityCalculator());
        }
    }
}
