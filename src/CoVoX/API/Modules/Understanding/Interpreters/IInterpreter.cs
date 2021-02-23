using System.Collections.Generic;

namespace API.Understanding
{
    internal interface IInterpreter
    {
        IReadOnlyList<Match> InterpretCommand(
            IReadOnlyList<Command> commands,
            string input);
    }
}
