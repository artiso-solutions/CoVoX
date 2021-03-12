using System;
using System.Threading.Tasks;
using Covox;

namespace LightSwitch
{
    class Program
    {
        // Get an Azure Cognitive Services subscription
        // here: https://azure.microsoft.com/try/cognitive-services/

        public const string YOUR_SUBSCRIPTION_KEY = "";
        public const string YOUR_REGION = "";

        static async Task Main()
        {
            var (turnOnLightCmd, turnOffLightCmd) = DefineCommands();

            var covox = CreateEngine();
            covox.RegisterCommands(turnOnLightCmd, turnOffLightCmd);

            covox.Recognized += (cmd, _) =>
            {
                if (cmd == turnOnLightCmd)
                    WriteLine("Light on", ConsoleColor.Yellow);
                else if (cmd == turnOffLightCmd)
                    WriteLine("Light off", ConsoleColor.DarkGray);
            };

            await covox.StartAsync();

            WriteLine(
                $"[LightSwitch] Press ENTER to exit{Environment.NewLine}" +
                $"[Covox] Say a command...{Environment.NewLine}");

            Console.ReadLine();

            WriteLine("[Covox] Stopping...");
            await covox.StopAsync();
        }

        private static (Command turnOnLightCmd, Command turnOffLightCmd) DefineCommands()
        {
            // Define the commands that can be invoked
            // with one or multiple voice triggers (in English)

            var turnOnLightCmd = new Command
            {
                Id = "TurnOnLight",
                VoiceTriggers = new[]
                {
                    "turn on the light",
                    "turn the light on",
                    "light on",
                    "on"
                }
            };

            var turnOffLightCmd = new Command
            {
                Id = "TurnOffLight",
                VoiceTriggers = new[]
                {
                    "turn off the light",
                    "turn the light off",
                    "light off",
                    "off"
                }
            };

            return (turnOnLightCmd, turnOffLightCmd);
        }

        private static CovoxEngine CreateEngine()
        {
            // Create an instance of the engine
            // providing the desired configuration

            var covox = new CovoxEngine(new Configuration
            {
                AzureConfiguration = AzureConfiguration.FromSubscription(
                    subscriptionKey: YOUR_SUBSCRIPTION_KEY,
                    region: YOUR_REGION),

                // Define all the languages that can be regognized:
                // Covox will take care of translating the spoken language into
                // the English voice triggers of the commands
                InputLanguages = new[] { "en-US", "de-DE", "it-IT", "es-ES" },
            });

            return covox;
        }

        private static void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
    }
}
