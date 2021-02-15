using System;
using System.Threading.Tasks;
using CustomCommands.Configuration.AppSettings;
using CustomCommands.Events;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CustomCommands
{
    class Program
    {
        private static async Task Main()
        {
            var configuration = AppSettingsHelper.GetSettings();
            var customEventHandlers = new CustomBaseEventHandlers(configuration);
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            
            var customCommands = new CustomCommandsClient(configuration, audioConfig, customEventHandlers);
                        
            await customCommands.StartListenForCommands();
            
            Console.ReadLine();
        }
    }
}
