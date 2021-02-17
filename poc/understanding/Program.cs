using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LanguageUnderstanding
{
    class Program
    {
        static async Task Main()
        {
            // Change to your LUIS app Id
            string luisAppId = "AppId";

            // Change to your LUIS subscription key
            string luisSubscriptionKey = "SubscriptionKey";

            //test
            var intent = "hi";

            //levenshtein
            var input = "Turn on the light";
            var target = "Turn on the light";
            Console.WriteLine(StringSimilarity.GetSimilarityLevenshtein(input, target));

            //ngram
            Console.WriteLine(StringSimilarity.GetSimilarityNGram(input, target));

            // -> difficult to predict intent by just comparing the strings. intent is formed by keywords: turn light on. The longer the sentence with missing / wrong words,
            // the bigger the calculated difference by string comparison will be. keyword parsing / intent recognition is needed.

            // text analystics
            TextAnalytics.AnalyseText(input);


            string score = await Luis.GetResult(luisAppId, luisSubscriptionKey, intent).ConfigureAwait(false);
            Console.WriteLine("Your Intent is: " + intent);
            Console.Write(string.Format("\n\rWith a Score of: {0} \n\r", score));
            Console.Write("\nPress any key to continue...");
            Console.Read();
        }
    }
    static class Luis
    {
        public static async Task<string> GetResult(string appIdLUIS, string subscriptionKeyLUIS, string intent)
        {
            if (string.IsNullOrEmpty(appIdLUIS) || string.IsNullOrEmpty(subscriptionKeyLUIS)) return null;

            var url = $"https://covox-cognitive-service.cognitiveservices.azure.com/luis/prediction/v3.0/apps/{appIdLUIS}/slots/staging/predict?subscription-key={subscriptionKeyLUIS}&verbose=true&show-all-intents=true&log=true&query={intent}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // TODO: fix Response object
                var result = JsonConvert.DeserializeObject<Response>(responseBody);

                return result.Prediction.Intents.FirstOrDefault().Value.Score.ToString();
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }

            return null;
        }
    }
}
