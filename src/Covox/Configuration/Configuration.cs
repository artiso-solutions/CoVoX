using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Covox.Validation;

#nullable disable

namespace Covox
{
    public class Configuration
    {
        [Required]
        public AzureConfiguration AzureConfiguration { get; set; }

        [Range(0, 1)]
        public double MatchingThreshold { get; set; } = 0.95;

        [NotEmpty, MustHaveValidLanguages]
        public IReadOnlyList<string> InputLanguages { get; set; }

        public IReadOnlyList<string> HotWords { get; set; }
    }
}
