namespace API
{
    public class Configuration
    {
        public AzureConfiguration AzureConfiguration { get; set; }
        public string[] InputLanguages { get; set; }
        public string[] HotWords { get; set; }
    }

    public class AzureConfiguration
    {
        public string SubscriptionKey { get; set; }
        public string Region { get; set; }
    }
}
