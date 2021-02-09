using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Newtonsoft.Json;
namespace LanguageUnderstanding
{
    public class Response
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("prediction")]
        public Prediction Prediction { get; set; }
    }

    //public class Prediction
    //{
    //    [JsonProperty("topIntent")] 
    //    public string TopIntent { get; set; }

    //    [JsonProperty("intents")] 
    //    public List<Intent> Intents { get; set; }

    //    [JsonProperty("entities")] 
    //    public string entity => null;

    //    [JsonProperty("sentiment")]
    //    public Sentiment Sentiment { get; set; }
    //}

    //public class Intent
    //{
    //    [JsonProperty("")]
    //    public string IntentName { get; set; }

    //    [JsonProperty("score")]
    //    public double ScoreValue { get; set; }
    //}
}