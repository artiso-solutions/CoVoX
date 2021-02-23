using System.Collections.Generic;
using API.Understanding;

namespace API
{
    public record RecognitionContext(
        string Input,
        string InputLanguage,
        double MatchScore,
        IReadOnlyList<Match> Candidates);
}
