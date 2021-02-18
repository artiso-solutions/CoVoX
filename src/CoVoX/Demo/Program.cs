using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API;

namespace Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new Configuration
            {
                AzureConfiguration = new AzureConfiguration
                {
                    SubscriptionKey = "",
                    Region = ""
                },
                InputLanguages = new[] { "de-DE" }
            };

            var commands = new List<Command>
            {
                new()
                {
                    Id = "TurnOnLight",
                    VoiceTriggers = new List<string>
                    {
                        "turn on the light",
                        "turn the light on",
                        "light on"
                    }
                },
                new()
                {
                    Id = "CloseWindow",
                    VoiceTriggers = new List<string>
                    {
                        "close the window",
                        "close window",
                        "window close"
                    }
                }
            };

            var covox = new Covox(configuration);
            covox.CommandDetected += Covox_CommandDetected;

            covox.RegisterCommands(commands);

            await covox.StartListening();

            Console.WriteLine("I'm listening...");

            Console.ReadLine();
        }

        private static void Covox_CommandDetected(object sender, Covox.CommandDetectedArgs e)
        {
            Console.WriteLine($"Recognized command: {e.Command.Id}");
        }
    }
}
