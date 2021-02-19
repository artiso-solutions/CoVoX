using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageUnderstanding
{
    class Demo
    {
        public static async Task Run()
        {
            //String Similarity
            var input = "Turn on the light";
            var target = "TURN_ON_LIGHT";
            var voiceTriggers = new List<string>();

            voiceTriggers.Add("Turn on Light");
            voiceTriggers.Add("Light on");
            voiceTriggers.Add("Turn Light");

            Command LightOnCommand = new Command();
            LightOnCommand.Id = target;
            LightOnCommand.VoiceTriggers = voiceTriggers;
            var commands = new List<Command>();
            commands.Add(LightOnCommand);
            var commandSimilarities = new List<CommandSimilarity>();

            foreach (var command in commands)
            {
                var result = StringSimilarity.CalculateHighestSimilarity(input, command);
                commandSimilarities.Add(new CommandSimilarity() { Command = command, Similarity = result });
            }

            var commandSimilarity = commandSimilarities.Max(x => x.Similarity);

            Console.WriteLine($"Hier steht die Command Similarity: {commandSimilarity}");

            // -> difficult to predict intent by just comparing the strings. intent is formed by keywords: turn light on. The longer the sentence with missing / wrong words,
            // the bigger the calculated difference by string comparison will be. keyword parsing / intent recognition is needed.

            //StringSimilarity.CombineMethods(input, target);

            // text analystics
            TextAnalytics.AnalyseText(input);

            // Change to your LUIS app Id
            //string luisAppId = "AppId";

            //// Change to your LUIS subscription key
            //string luisSubscriptionKey = "SubscriptionKey";

            ////test
            //var intent = "hi";

            //string score = await Luis.GetResult(luisAppId, luisSubscriptionKey, intent).ConfigureAwait(false);
            //Console.WriteLine("Your Intent is: " + intent);
            //Console.Write(string.Format("\n\rWith a Score of: {0} \n\r", score));
            //Console.Write("\nPress any key to continue...");
            //Console.Read();
        }
    }
}
