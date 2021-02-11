namespace SpeechTranslation
{
    abstract class AbstractScenario
    {
        public string SubscriptionKey { get; }

        public string Region { get; }

        public AbstractScenario(string subscriptionKey, string region)
        {
            SubscriptionKey = subscriptionKey;
            Region = region;
        }
    }
}
