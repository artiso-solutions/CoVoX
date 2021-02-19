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
             */
            Test.With(new DummySimilarityCalculator());
        }
    }
}
