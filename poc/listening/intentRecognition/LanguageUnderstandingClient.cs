using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using IntentRecognition.Settings;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace IntentRecognition
{
    /// <summary>
    /// Make calls against <remarks>your</remarks> LUIS app <seealso cref="http://www.luis.ai/"/>
    /// </summary>
    public class LanguageUnderstandingClient
    {
        private readonly LanguageUnderstandingClientConfiguration _configuration;
        
        public LanguageUnderstandingClient(LanguageUnderstandingClientConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Get intents by using a IntentRecognizer object
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetIntentWithIntentRecognizer()
        {
            var config = SpeechConfig.FromSubscription(_configuration.PredictionKey, _configuration.Region);

            // config.SpeechRecognitionLanguage = "en-US"; - Default

            using var recognizer = new IntentRecognizer(config);
            
            // Creates a Language Understanding model using the LUIS AppId
            var model = LanguageUnderstandingModel.FromAppId(_configuration.AppId);
                
            // To add only specific intents
            // recognizer.AddIntent(model, "intent name", "intent_uid");

            // To add all of the defined intents from a LUIS model
            recognizer.AddAllIntents(model);

            // Starts recognizing.
            Console.WriteLine("Say something...");

            var result = await recognizer.RecognizeOnceAsync();
                
            switch (result.Reason)
            {
                case ResultReason.RecognizedIntent:
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent Id: {result.IntentId}.");
                    return result.Properties.GetProperty(PropertyId
                        .LanguageUnderstandingServiceResponse_JsonResult);

                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    Console.WriteLine($"    Intent not recognized.");
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }

                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// Do the SpeechToText recognition by using SpeechRecognizer and calling the REST endpoint of the LUIS app <seealso cref="http://www.luis.ai/"/>
        /// </summary>
        /// <param name="speechRecognizerConfig"></param>
        /// <returns></returns>
        public async Task<string> GetIntentWithRestClient(SpeechRecognitionClientConfiguration  speechRecognizerConfig)
        {
            var (text, _) = await RecognizeSpeech(speechRecognizerConfig);
            
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.PredictionKey);

            queryString["query"] = text;
            // These optional request parameters are set to their default values
            queryString["verbose"] = "true";
            queryString["show-all-intents"] = "true";
            queryString["staging"] = "false";
            queryString["timezoneOffset"] = "0";

            var predictionEndpointUri = String.Format("{0}luis/prediction/v3.0/apps/{1}/slots/{2}/predict?{3}",
                _configuration.EndPoint,
                _configuration.AppId,
                _configuration.Env.ToString().ToLower(),
                queryString);

            var response = await client.GetAsync(predictionEndpointUri);

            var strResponseContent = await response.Content.ReadAsStringAsync();

            return strResponseContent;
        }

        #region GetIntentRestClient

        private async Task<(string Text, string DetectedLanguage)> RecognizeSpeech(SpeechRecognitionClientConfiguration  speechRecognizerConfig)
        {
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new[] { "en-US" });

            var subscriptionKey = speechRecognizerConfig.SubscriptionKey;
            var region = speechRecognizerConfig.Region;
            
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig);
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.Canceled)
                throw new Exception("Api Communication Failure");

            var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result).Language;
            return (result.Text, detectedLanguage);
        }


        #endregion
    }
}
