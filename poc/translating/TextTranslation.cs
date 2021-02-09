using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TextTranslate
{
    public class TextTranslation
    {
        private static readonly string Endpoint = "https://api.cognitive.microsofttranslator.com/";
        
        public static async Task<List<TranslationResponse.Result>> TranslateToEn(string subscriptionKey, string region, string textToTranslate)
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

            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", region);

            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var result = JsonConvert.DeserializeObject<List<TranslationResponse.Result>>(responseBody);

            return result;
        }
    }
}
