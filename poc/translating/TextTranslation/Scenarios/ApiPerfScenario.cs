using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Spectre.Console;

namespace TextTranslate
{
    class ApiPerfScenario : AbstractScenario
    {
        public ApiPerfScenario(string subscriptionKey, string region)
            : base(subscriptionKey, region)
        {
        }

        public async Task Run()
        {
            var testCases = new[]
            {
                "Ciao mondo",
                "Giù",
                "Accendi la luce",
                "42 è la risposta alla domanda fondamentale",
            };

            var progress = AnsiConsole.Progress()
                .AutoRefresh(true)
                .AutoClear(true)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                });

            var testResults = new List<(string, List<(string, TimeSpan)>)>();

            await progress.StartAsync(async ctx =>
            {
                const int iterations = 3;
                var apiCallsCount = testCases.Length * iterations + 1 /* cold-start */;

                var testingProgress = ctx.AddTask($"Testing", new ProgressTaskSettings { MaxValue = apiCallsCount });

                // Try to avoid cold-start
                var wakeUp = "Svegliati";
                var wakeUpResult = await TranslateWithPerf(wakeUp);

                testingProgress.Increment(1);
                testResults.Add((wakeUp, new List<(string, TimeSpan)> { wakeUpResult }));

                foreach (var testCase in testCases)
                {
                    var translations = new List<(string, TimeSpan)>();

                    for (int i = 0; i < iterations; i++)
                    {
                        var result = await TranslateWithPerf(testCase);
                        translations.Add(result);

                        testingProgress.Increment(1);
                    }

                    testResults.Add((testCase, translations));
                }
            });

            PrintAsTable(testResults);
        }

        private async Task<(string, TimeSpan)> TranslateWithPerf(string input)
        {
            var requestContent = TextTranslation.AsRequestContent(input);

            var sw = Stopwatch.StartNew();
            var json = await TextTranslation.TranslateToEnRaw(SubscriptionKey, Region, requestContent);
            sw.Stop();

            var results = TextTranslation.ParseAsResults(json);

            var best = results.OrderByDescending(x => x.DetectedLanguage.Score).First();
            var translation = best.Translations.OrderByDescending(x => x.Text).First().Text;

            return (translation, sw.Elapsed);
        }

        private static void PrintAsTable(List<(string, List<(string, TimeSpan)>)> testResults)
        {
            var table = new Table { Border = TableBorder.SimpleHeavy };

            table
                .AddColumn("")
                .AddColumn("Sentence")
                .AddColumn("Time");

            for (int i = 0; i < testResults.Count; i++)
            {
                if (i != 0) table.AddEmptyRow();

                var test = testResults[i];

                var (testCase, translations) = test;
                table.AddRow("Input", testCase, "");

                foreach (var (translation, elapsed) in translations)
                    table.AddRow("", translation, elapsed.ToString());
            }

            AnsiConsole.Render(table);
        }
    }
}
