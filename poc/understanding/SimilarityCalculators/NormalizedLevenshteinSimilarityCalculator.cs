using F23.StringSimilarity;

namespace LanguageUnderstanding
{
    class NormalizedLevenshteinSimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            var normalizedLevenshtein = new NormalizedLevenshtein();
            var distance = normalizedLevenshtein.Distance(input, target);
            // WARNING: StringSimilarity returns values from 0 to "infinite",
            // so it doesn't match with the 0..1 signature.
            return 1.0 - distance;
        }
    }
}
