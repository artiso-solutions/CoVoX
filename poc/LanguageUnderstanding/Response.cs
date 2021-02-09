using System.Collections.Generic;
using Newtonsoft.Json;
namespace LanguageUnderstanding
{
    public class Response
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("prediction")]
        public Predicion Prediction { get; set; }

        //[JsonProperty("score")]
        //public double Score { get; set; }
    }
    public class Predicion
    {
        [JsonProperty("topIntent")]
        public string TopIntent { get; set; }

        [JsonProperty("intents")]
        public List<Score> Score { get; set; }
    }

    public class Score
    {
        [JsonProperty("")]
        public double IntentName { get; set; }

        [JsonProperty("score")]
        public double ScoreValue { get; set; }
    }
}