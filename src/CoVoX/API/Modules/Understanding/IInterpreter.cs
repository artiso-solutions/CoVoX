using System.Collections.Generic;

namespace API.Modules
{
    public interface IInterpreter
    {
        Command InterpretCommand(IReadOnlyList<Command> commands, double matchingThreshold, string text);
    }
}