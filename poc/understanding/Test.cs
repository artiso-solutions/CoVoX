using System;

namespace LanguageUnderstanding
{
    class Test
    {
        public static void With(ISimilarityCalculator sc)
        {
            Assert(sc is not null);

            // This is the command's voice trigger.
            const string target = "Turn on the light";

            // The input is what the speech-to-translated-text will recognize
            var input = "Turn on the light";
            var exactMatchScore = sc.Calculate(target, input);
            // Each of these target scores (0.99) are what we define as matching threshold
            Assert(exactMatchScore >= 0.99, "Exact match");

            var similarScore = sc.Calculate(target, "Turn on light");
            Assert(similarScore >= 0.90, "Input and target are quite similar");

            var allUpperScore = sc.Calculate(target, "TURN ON THE LIGHT");
            Assert(allUpperScore >= 0.95, "Input is just upper-case target");

            var allLowerScore = sc.Calculate(target, "turn on the light");
            Assert(allLowerScore >= 0.95, "Input is just lower-case target");

            var leanScore = sc.Calculate(target, "On light");
            Assert(leanScore >= 0.85, "Input partial target");

            var noMatchScore = sc.Calculate(target, "abcdefghijklmnopqrstuvwxyz");
            Assert(noMatchScore < 10, "Input is just wrong");

            var reorderedScore = sc.Calculate(target, "light Turn the on");
            Assert(reorderedScore < 90, "Input is reordered compared to target");

            var differentSubjectScore = sc.Calculate(target, "Turn on the stove");
            Assert(differentSubjectScore > 80, "Input is not quite accurate");
            Assert(differentSubjectScore < 95, "Input is not that accurate");
        }

        static void Assert(bool condition, string message = null)
        {
            if (!condition) throw new Exception(message ?? "Assert failed");
        }
    }
}
