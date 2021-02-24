using System;
using System.Threading.Tasks;
using LanguageUnderstanding.SimilarityCalculators;
using Spectre.Console;

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

            //RunTests();
            RunCalculateTests();
        }

        static void RunCalculateTests()
        {
            var target = "Turn on the Light";
            var inputs = new string[]
            {
                "Turn on the Light",
                "Turn the Light on.",
                "Light on",
                "Turn off the Light",
                "Take a picture of me"
            };
            foreach (var input in inputs)
            {
                var table = new Table();
                table.AddColumn("Algorithm");
                table.AddColumn("Library");
                table.AddColumn("Normalized");
                table.AddColumn("Target");
                table.AddColumn("Input");
                table.AddColumn("Score");

                table.Border = TableBorder.Markdown;

                var score = new FuzzyWeightedRatioCalculator().Calculate(target, input);
                table.AddRow("FuzzyWeightedRatio", "FuzzySharp", "yes", $"{target}", $"{input}", $"{score}");

                score = new CosineSimilarityCalculator().Calculate(target, input);
                table.AddRow("CosineSimilarity", "StringSimilarity", "yes", $"{target}", $"{input}", $"{score}");

                score = new JaroWinklerSimilarityCalculator().Calculate(target, input);
                table.AddRow("JaroWinklerSimilarity", "StringSimilarity", "yes", $"{target}", $"{input}", $"{score}");

                score = new NGramSimilarityCalculator().Calculate(target, input);
                table.AddRow("NGramSimilarity", "StringSimilarity", "yes", $"{target}", $"{input}", $"{score}");
                
                score = new NormalizedLevenshteinSimilarityCalculator().Calculate(target, input);
                table.AddRow("NormalizedLevenshteinSimilarity", "StringSimilarity", "yes", $"{target}", $"{input}", $"{score}");

                score = new MetricLCSSimilarityCalculator().Calculate(target, input);
                table.AddRow("MetricLCSSimilarity", "StringSimilarity", "yes", $"{target}", $"{input}", $"{score}");

                score = new QGramSimilarityCalculator().Calculate(target, input);
                table.AddRow("QGramSimilarity", "StringSimilarity", "no", $"{target}", $"{input}", $"{score}");

                score = new DamerauSimilarityCalculator().Calculate(target, input);
                table.AddRow("DamerauSimilarity", "StringSimilarity", "no", $"{target}", $"{input}", $"{score}");

                score = new LevenshteinSimilarityCalculator().Calculate(target, input);
                table.AddRow("LevenshteinSimilarity", "StringSimilarity", "no", $"{target}", $"{input}", $"{score}");

                score = new LongestCommonSubsequenceSimilarityCalculator().Calculate(target, input);
                table.AddRow("LongestCommonSubsequenceSimilarity", "StringSimilarity", "no", $"{target}", $"{input}", $"{score}");

                AnsiConsole.Render(table);
            }
            
        }

        static void RunTests()
        {
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
