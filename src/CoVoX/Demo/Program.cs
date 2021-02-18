using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Demo
{
    class Program
    {
        private static readonly string _subscriptionKey = "";
        private static readonly string _region = "";

        static async Task Main(string[] args)
        {
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

        private static void Covox_CommandDetected(object sender, Covox.CommandDetectedArgs e)
        {
            Console.WriteLine($"Recognized command: {e.Command.Id}");
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
                Console.WriteLine($"CoVoX Sample App{Environment.NewLine}Copyright (c) artiso solutions GmbH{Environment.NewLine}{Environment.NewLine}https://github.com/artiso-solutions/CoVoX{Environment.NewLine}");

                var configuration = new Configuration
                {
                    AzureConfiguration = new AzureConfiguration
                    {
                        SubscriptionKey = _subscriptionKey,
                        Region = _region
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
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }
    }
}
