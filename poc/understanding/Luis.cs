using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LanguageUnderstanding
{
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
