using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Covox
{
    public class Configuration
    {
        [Required]
        public AzureConfiguration AzureConfiguration { get; set; }

        [Required]
        [Range(0, 1)]
        public double MatchingThreshold { get; set; }

        [Required]
        public IReadOnlyList<string> InputLanguages { get; set; }

        public IReadOnlyList<string> HotWords { get; set; }
    }

    public class AzureConfiguration
    {
        [Required]
        public string SubscriptionKey { get; set; }
        
        [Required]
        public string Region { get; set; }
    }
}
