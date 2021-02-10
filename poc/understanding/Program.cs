using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace LanguageUnderstanding
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Change to your LUIS app Id
            string luisAppId = "dd8e2d7d-2ce4-49dd-afa2-375785f0d61e";

            // Change to your LUIS subscription key
            string luisSubscriptionKey = "d1510f61f03747969f823ef1e8cf39ac";

            //test
            var intent = "hi";

            string region = await Luis.GetResult(luisAppId, luisSubscriptionKey, intent).ConfigureAwait(false);
            Console.Write(string.Format("\n\rLUIS region: {0} \n\r", region));
            Console.Write("\nPress any key to continue...");
            Console.Read();
        }
    }
    static class Luis
    {
        public static async Task<string> GetResult(string appIdLUIS, string subscriptionKeyLUIS, string intent)
        {
            if (String.IsNullOrEmpty(appIdLUIS) || String.IsNullOrEmpty(subscriptionKeyLUIS)) return string.Empty;

            // queryString
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //todo format url with these
            string appIdWithSubscriptionKey = string.Join("|", appIdLUIS, subscriptionKeyLUIS);

            using (var client = new HttpClient())
            {
                var url = "https://covox-cognitive-service.cognitiveservices.azure.com/luis/prediction/v3.0/apps/dd8e2d7d-2ce4-49dd-afa2-375785f0d61e/slots/staging/predict?subscription-key=d1510f61f03747969f823ef1e8cf39ac&verbose=true&show-all-intents=true&log=true&query=";
                var urlWithIntent = url += intent;

                try
                {
                    using (var response = client.GetAsync(urlWithIntent, HttpCompletionOption.ResponseContentRead, CancellationToken.None).Result)
                    {
                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();

                            //TODO: fix Response object
                            var result = JsonConvert.DeserializeObject<List<Response>>(responseBody);
                        }
                        else
                        {
                            Debugger.Log(0, "", "401 " + url + "\n\r");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Log(0, "exception", ex.Message);
                }
            }

            return String.Empty;
        }
    }
}