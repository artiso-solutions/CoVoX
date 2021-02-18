using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Modules;

namespace API
{
    public interface IUnderstandingModule
    {
        event EventHandler<Interpreter.CommandRecognizedArgs> CommandRecognized;
        void RegisterCommands(List<Command> commands);
        IReadOnlyList<Command> GetRegisteredCommands();
        Task StartCommandDetection();

        Task StopCommandDetection();
    }
}