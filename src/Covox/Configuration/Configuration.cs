using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Covox.Validation;

#nullable disable

namespace Covox
{
    public class Configuration
    {
        public Configuration()
        {
            MatchingThreshold = 0.95;
            AudioSource = AudioSource.FromDefaultMicrophone();
        }

        [Required]
        public AzureConfiguration AzureConfiguration { get; set; }

        [Range(0, 1)]
        public double MatchingThreshold { get; set; }

        [NotEmpty, MustHaveValidLanguages]
        public IReadOnlyList<string> InputLanguages { get; set; }

        [Required]
        public AudioSource AudioSource { get; set; }
    }
}
