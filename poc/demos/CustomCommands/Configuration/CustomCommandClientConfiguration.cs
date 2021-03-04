namespace CustomCommands.Configuration
{
    /// <summary>
    /// Get the configuration property from section "Settings" of your CustomCommands app
    /// </summary>
    internal class CustomCommandClientConfiguration
    {
        public string SubscriptionKey { get; set; }

        public string AppId { get; set; }

        public string Region { get; set; }
    }
}
