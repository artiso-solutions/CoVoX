using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TextTranslate
{
    public class TextTranslation
    {
        private static readonly string Endpoint = "https://api.cognitive.microsofttranslator.com/";
        private static readonly Uri ApiUri = new Uri($"{Endpoint}/translate?api-version=3.0&to=en");

        private static readonly HttpClient _client = new();

        public static async Task<List<TranslationResponse.Result>> TranslateToEn(
            string subscriptionKey,
            string region,
            string textToTranslate)
        {
            var requestContent = AsRequestContent(textToTranslate);
            var json = await TranslateToEnRaw(subscriptionKey, region, requestContent);

            var results = ParseAsResults(json);
            return results;
        }

        public static async Task<string> TranslateToEnRaw(
            string subscriptionKey,
            string region,
            HttpContent requestContent)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = ApiUri,
                Content = requestContent
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", region);

            using var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            return json;
        }

        public static HttpContent AsRequestContent(string textToTranslate)
        {
            var body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            return requestContent;
        }

        public static List<TranslationResponse.Result> ParseAsResults(string json)
        {
            var results = JsonConvert.DeserializeObject<List<TranslationResponse.Result>>(json)
                .OrderByDescending(x => x.DetectedLanguage.Score)
                .ToList();

            return results;
        }
    }
}
