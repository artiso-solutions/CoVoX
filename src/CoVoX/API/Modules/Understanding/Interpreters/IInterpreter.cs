using System.Collections.Generic;

namespace API.Modules
{
    internal interface IInterpreter
    {
        IReadOnlyList<Match> InterpretCommand(
            IReadOnlyList<Command> commands,
            string input);
    }
}
