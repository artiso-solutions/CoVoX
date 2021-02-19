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
            Log.Debug(Command != null ? $"Detected command: {Command?.Id}" : $"No matching command found for '{text}'");
        }
    }
}