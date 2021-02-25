using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Covox;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

namespace LightSwitchDemo
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
                    AzureConfiguration = AzureConfiguration.FromFile(),
                    InputLanguages = new[] { "de-DE", "it-IT", "es-ES" },
                    MatchingThreshold = 0.8
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

                var serilogLogger = new SerilogLoggerProvider(Log.Logger).CreateLogger(nameof(MainWindow));

                var covox = new CovoxEngine(configuration, serilogLogger);
                covox.Recognized += Covox_Recognized;

                covox.RegisterCommands(commands);

                await covox.StartAsync();
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

        private void Covox_Recognized(Command command, RecognitionContext context)
        {
            if (command == null) return;

            try
            {
                if (command.Id.Equals(TurnOffLight))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => this.lightGrid.Background = new SolidColorBrush(Colors.Black)));
                }
                else if (command.Id.Equals(TurnOnLight))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => this.lightGrid.Background = new SolidColorBrush(Colors.Yellow)));
                }
                else if (command.Id.Equals(TurnOnLightRed))
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
