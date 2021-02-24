using System.Collections.Generic;
using Covox.Understanding;

namespace Covox
{
    public record RecognitionContext(
        string Input,
        string InputLanguage,
        double MatchScore,
        IReadOnlyList<Match> Candidates);
}
