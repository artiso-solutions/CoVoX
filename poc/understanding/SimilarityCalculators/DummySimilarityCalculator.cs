namespace LanguageUnderstanding
{
    class DummySimilarityCalculator : ISimilarityCalculator
    {
        public double Calculate(string target, string input)
        {
            return target.ToLower().Contains(input.ToLower()) ? 1 : 0;
        }
    }
}
