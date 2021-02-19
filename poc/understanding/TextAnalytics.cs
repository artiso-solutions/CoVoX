using System;
using Azure;
using Azure.AI.TextAnalytics;

namespace LanguageUnderstanding
{
    public class TextAnalytics
    {
        private static readonly string textAnalyticsApiKey = "";
        private static readonly string textAnalyticsEndpoint = "https://textanalyticscovox.cognitiveservices.azure.com/";

        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(textAnalyticsApiKey);
        private static readonly Uri endpoint = new Uri(textAnalyticsEndpoint);

        public static string AnalyseText(string input)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);

            DetectLanguage(client, input);
            KeyWordExtraction(client, input);
            GetNamedEntities(client, input);

            return "";
        }

        public static void DetectLanguage(TextAnalyticsClient client, string input)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage(input);
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }

        public static void KeyWordExtraction(TextAnalyticsClient client, string input)
        {
            var response = client.ExtractKeyPhrases(input);

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }

        public static void GetNamedEntities(TextAnalyticsClient client, string input)
        {
            var response = client.RecognizeEntities(input);
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2}");
            }
        }
    }
}
