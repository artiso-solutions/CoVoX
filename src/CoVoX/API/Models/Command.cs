using System.Collections.Generic;

namespace API
{
    public class Command
    {
        public string Id { get; set; }
        
        internal double MatchScore { get; set; } // TODO: remove from Command type.
        
        public IReadOnlyList<string> VoiceTriggers { get; init; }
    }
}
