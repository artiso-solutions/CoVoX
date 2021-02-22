using System.Collections.Generic;
using System.Linq;

namespace API.Modules.Understanding.Interpreters
{
    public class SimpleInterpreter : IInterpreter
    {
        public (Command, IReadOnlyList<Command>) InterpretCommand(IReadOnlyList<Command> commands, double matchingThreshold,
            string text)
        {
            return (commands.FirstOrDefault(x => x.VoiceTriggers.Any(y => text.ToLower().Contains(y.ToLower()))), commands);
        }
    }
}
