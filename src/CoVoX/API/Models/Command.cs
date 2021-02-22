using System.Collections.Generic;

namespace API
{
    public class Command
    {
        public string Id { get; set; }
        public double MatchScore { get; set; }
        public IReadOnlyList<string> VoiceTriggers { get; init; }
    }

    public class DetectionContext
    {
        public string Input { get; set; }
        public string InputLanguage { get; set; }
        public IReadOnlyList<Command> Candidates { get; set; }
    }
}
