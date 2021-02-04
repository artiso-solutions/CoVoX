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
        private static readonly string SubscriptionKey = "SubscriptionKey";
        private static readonly string Endpoint = "https://api.cognitive.microsofttranslator.com/";
        private static readonly string Region = "Region";

        public static async Task<string> TranslateToEn(string textToTranslate)
        {
            var route = $"/translate?api-version=3.0&to=en";
            var body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Endpoint + route),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", Region);

            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var result = JsonConvert.DeserializeObject<List<TranslationResponse.Result>>(responseBody);

            return result.First().Translations.First().Text;
        }
    }
}
