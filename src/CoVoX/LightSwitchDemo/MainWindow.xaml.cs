using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using API;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LigthSwitchDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TurnOffLight = "TurnOffLight";
        private const string TurnOnLight = "TurnOnLight";
        private const string TurnOnLightRed = "TurnOnLightRed";

        public MainWindow()
        {
			// Bla
            InitializeComponent();
            SetupStaticLogger();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await RunApp();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
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

        private async Task RunApp()
        {
            try
            {
                var configuration = new Configuration
                {
                    AzureConfiguration = SecretsHelper.GetSecrets(),
                    InputLanguages = new[] { "de-DE", "it-IT", "es-ES" }
                };

                var commands = new List<Command>
                {
                    new()
                    {
                        Id = TurnOnLight,
                        VoiceTriggers = new List<string>
                        {
                            "turn on the light",
                            "turn the light on",
                            "light on"
                        }
                    },
                    new()
                    {
                        Id = TurnOffLight,
                        VoiceTriggers = new List<string>
                        {
                            "turn off the light",
                            "turn the light off",
                            "light off"
                        }
                    },
                    new()
                    {
                        Id = TurnOnLightRed,
                        VoiceTriggers = new List<string>
                        {
                            "color to red",
                            "red color",
                            "turn light red"
                        }
                    }
                };

                var covox = new Covox(configuration);
                covox.CommandDetected += Covox_CommandDetected;

                covox.RegisterCommands(commands);

                await covox.StartListening();
                Log.Debug("I'm listening...");
                while (true)
                {
                    await Task.Delay(2000);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }

        private void Covox_CommandDetected(object sender, Covox.CommandDetectedArgs e)
        {
            if (e.Command == null) return;

            try
            {
                if (e.Command.Id.Equals(TurnOffLight))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => this.lightGrid.Background = new SolidColorBrush(Colors.Black)));
                }
                else if (e.Command.Id.Equals(TurnOnLight))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => this.lightGrid.Background = new SolidColorBrush(Colors.Yellow)));
                }
                else if (e.Command.Id.Equals(TurnOnLightRed))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => this.lightGrid.Background = new SolidColorBrush(Colors.Red)));
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }
    }
}
