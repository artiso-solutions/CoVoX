using System.Collections.Generic;

namespace API.Modules
{
    public interface IInterpreter
    {
        Command InterpretCommand(IReadOnlyList<Command> commands, string text);
    }
}