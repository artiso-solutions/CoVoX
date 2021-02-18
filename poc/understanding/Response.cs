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
}