using System.Collections.Generic;

namespace API
{
    internal interface IUnderstandingModule
    {
        IReadOnlyList<Command> Commands { get; }

        void RegisterCommands(IEnumerable<Command> commands);

        (Match bestMatch, IReadOnlyList<Match> candidates) Understand(string input);
    }
}
