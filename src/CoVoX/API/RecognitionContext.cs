using System.Collections.Generic;

namespace API
{
    public record RecognitionContext(
        string Input,
        string InputLanguage,
        double MatchScore,
        IReadOnlyList<Match> Candidates);
}
