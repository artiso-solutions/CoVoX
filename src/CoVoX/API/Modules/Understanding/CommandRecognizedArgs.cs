using System;
using System.Collections.Generic;
using Serilog;

namespace API.Modules
{
    public class CommandRecognizedArgs : EventArgs
    {
        public Command Command { get; }

        public CommandRecognizedArgs(IInterpreter interpreter, IReadOnlyList<Command> commands, string text)
        {
            Command = interpreter.InterpretCommand(commands, text);
            if (Command != null)
            {
                Log.Debug($"Detected command: {Command?.Id}");
            }
            else
            {
                Log.Debug($"No matching command found for '{text}'");
            }
        }
    }
}