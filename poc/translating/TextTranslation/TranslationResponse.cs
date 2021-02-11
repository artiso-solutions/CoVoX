using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextTranslate
{
    public class TranslationResponse
    {
        public class DetectedLanguage
        {
            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("score")]
            public double Score { get; set; }
        }

        public class Translation
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }
        }

        public class Result
        {
            [JsonProperty("detectedLanguage")]
            public DetectedLanguage DetectedLanguage { get; set; }

            [JsonProperty("translations")]
            public List<Translation> Translations { get; set; }
        }
    }
}
