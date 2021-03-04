namespace IntentRecognition.Settings
{
    public class LanguageUnderstandingClientConfiguration
    {
        /// <summary>
        /// Application Endpoint, such as "https://westus.api.cognitive.microsoft.com/
        /// </summary>
        public string EndPoint { get; init; }

        /// <summary>
        /// ApplicationId, uuid of your application
        /// See Azure Resource of your <see cref="https://www.luis.ai/applications/"/>, it's the uuid set in your endpoint after apps/
        /// i.e. https://westus.api.cognitive.microsoft.com/luis/prediction/v3.0/apps/UUID/slots/...
        /// </summary>
        public string AppId { get; init; }

        /// <summary>
        /// It's the subscription key of your Luis App. 
        /// See Azure Resource of your <see cref="https://www.luis.ai/applications/"/>, it's one of the 2 keys (primary and secondary)
        /// </summary>
        public string PredictionKey { get; init; }

        /// <summary>
        /// Environment ("production", "staging")
        /// </summary>
        public  LanguageUnderstandingClientEnv  Env { get; init; }

        /// <summary>
        /// Luis App Region config
        /// </summary>
        public string Region { get; init; }
    }
}
