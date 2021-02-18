using System.Collections.Generic;
using System.Linq;

namespace API.Modules.Understanding.Interpreters
{
    public class SimpleInterpreter : IInterpreter
    {
        public Command InterpretCommand(IReadOnlyList<Command> commands, string text)
        {
            return commands.FirstOrDefault(x => x.VoiceTriggers.Any(y => text.ToLower().Contains((string) y.ToLower())));
        }
    }
}