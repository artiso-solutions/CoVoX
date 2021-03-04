using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Covox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace WpfDemo
{
    public partial class MainWindow : Window
    {
        private static readonly Command TurnOnLightCmd = new()
        {
            Id = "TurnOnLight",
            VoiceTriggers = new List<string>
            {
                "turn on the light",
                "turn the light on",
                "light on"
            }
        };

        private static readonly Command TurnOffLightCmd = new()
        {
            Id = "TurnOffLight",
            VoiceTriggers = new List<string>
            {
                "turn off the light",
                "turn the light off",
                "light off"
            }
        };

        private static readonly Command OpenWindowCmd = new()
        {
            Id = "OpenWindow",
            VoiceTriggers = new List<string>
            {
                "open the window",
                "open window",
                "window open"
            }
        };

        private static readonly Command CloseWindowCmd = new()
        {
            Id = "CloseWindow",
            VoiceTriggers = new List<string>
            {
                "close the window",
                "close window",
                "window close"
            }
        };

        public MainWindow()
        {
            InitializeComponent();
            SetupStaticLogger();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RunApp();
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
                    AzureConfiguration = AzureConfiguration.FromEnvironmentVariables(),
                    InputLanguages = new[] { "en-US", "de-DE", "it-IT", "es-ES" },
                    MatchingThreshold = 0.85
                };

                var logger = new SerilogLoggerFactory(Log.Logger).CreateLogger<CovoxEngine>();
                var covox = new CovoxEngine(logger, configuration);

                covox.RegisterCommands(TurnOnLightCmd, TurnOffLightCmd, OpenWindowCmd, CloseWindowCmd);

                covox.Recognized += Covox_Recognized;
                covox.Unrecognized += Covox_Unrecognized;

                await covox.StartAsync();
                Log.Debug("I'm listening...");
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
        }

        private void Covox_Unrecognized(List<(string input, string inputLanguage)> inputs)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var input in inputs)
                sb.AppendLine($"\t{input}");

            Log.Warning($"Unable to recognize input:\n{sb}");
        }

        private void Covox_Recognized(Command command, RecognitionContext context)
        {
            try
            {
                if (command == TurnOnLightCmd)
                    UpdateUI(() =>
                    {
                        LightOn.Visibility = Visibility.Visible;
                        LightOff.Visibility = Visibility.Hidden;
                    });
                else if (command == TurnOffLightCmd)
                    UpdateUI(() =>
                    {
                        LightOn.Visibility = Visibility.Hidden;
                        LightOff.Visibility = Visibility.Visible;
                    });
                else if (command == OpenWindowCmd)
                    UpdateUI(() =>
                    {
                        WindowOpen.Visibility = Visibility.Visible;
                        WindowClose.Visibility = Visibility.Hidden;
                    });
                else if (command == CloseWindowCmd)
                    UpdateUI(() =>
                    {
                        WindowOpen.Visibility = Visibility.Hidden;
                        WindowClose.Visibility = Visibility.Visible;
                    });
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }

            // Local functions.

            static void UpdateUI(Action uiAction)
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    uiAction);
            }
        }
    }
}
