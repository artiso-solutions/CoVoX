using System;
using System.Collections.Generic;
using Serilog;

namespace API.Modules
{
    public class CommandRecognizedArgs : EventArgs
    {
        public Command Command { get; }
        public DetectionContext DetectionContext { get; }

        public CommandRecognizedArgs(IInterpreter interpreter, IReadOnlyList<Command> commands, double matchingThreshold,
            string text, string inputLanguage)
        {
            DetectionContext = new DetectionContext();
            var (command, candidates) = interpreter.InterpretCommand(commands, matchingThreshold, text);
            Command = command;
            DetectionContext = new DetectionContext
            {
                Input = text,
                InputLanguage = inputLanguage,
                Candidates = candidates
            };

            Log.Debug(Command != null
                ? $"Detected command: {Command?.Id} ('{inputLanguage}' Score: {command.MatchScore * 100}%)"
                : $"No matching command found for '{text}' ('{inputLanguage}')");
        }
    }
}
