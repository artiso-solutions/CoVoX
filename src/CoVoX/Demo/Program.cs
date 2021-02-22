using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("CoVoX Sample App" +
                              Environment.NewLine +
                              "Copyright (c) artiso solutions GmbH" +
                              Environment.NewLine +
                              Environment.NewLine +
                              "https://github.com/artiso-solutions/CoVoX" +
                              Environment.NewLine);

            SetupStaticLogger();

            try
            {
                Log.Debug("Starting CoVoX demo app");
                await RunApp();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SetupStaticLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static async Task RunApp()
        {
            try
            {
                var configuration = new Configuration
                {
                    AzureConfiguration = SecretsHelper.GetSecrets(),
                    InputLanguages = new[] {"en-US", "de-DE", "it-IT"},
                    MatchingThreshold = 0.99
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
                        Id = "TurnOffLight",
                        VoiceTriggers = new List<string>
                        {
                            "turn off the light",
                            "turn the light off",
                            "light off"
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
                    },
                    new()
                    {
                        Id = "OpenWindow",
                        VoiceTriggers = new List<string>
                        {
                            "open the window",
                            "open window",
                            "window open"
                        }
                    }
                };

                var covox = new Covox(configuration);
                covox.CommandDetected += Covox_CommandDetected;

                covox.RegisterCommands(commands);

                await covox.StartAsync();
                Console.WriteLine("I'm listening...");
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }

        private static void Covox_CommandDetected(object sender, Covox.CommandDetectedArgs e)
        {
            Console.WriteLine($"Recognized command: {e.Command.Id}");
        }
    }
}
