using System.Collections.Generic;

namespace API
{
    public class Command
    {
        public string Id { get; set; }
        internal double MatchScore { get; set; }
        public IReadOnlyList<string> VoiceTriggers { get; init; }
    }
}
