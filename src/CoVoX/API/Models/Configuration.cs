using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Configuration
    {
        [Required]
        public AzureConfiguration AzureConfiguration { get; set; }

        [Required]
        [Range(0, 1)]
        public double MatchingThreshold { get; set; }

        [Required]
        public string[] InputLanguages { get; set; }

        public string[] HotWords { get; set; }
    }

    public class AzureConfiguration
    {
        public string SubscriptionKey { get; set; }
        public string Region { get; set; }
    }
}
