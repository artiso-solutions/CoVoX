using System;
using System.Threading.Tasks;

namespace SpeechToText
{
    class Program
    {
        private static readonly string SubscriptionKey = "SubscriptionKey";
        private static readonly string Region = "Region";
        
        static async Task Main(string[] args)
        {
            var showMenu = true;
            while (showMenu)
            {
                showMenu = await Menu();
            }
        }

        private static async Task<bool> Menu()
        {
            Console.Clear();
            Console.WriteLine("Choose recognition mode:");
            Console.WriteLine("1) Single-shot");
            Console.WriteLine("2) Continuous");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await SpeechRecognition.RecognizeSpeech(SubscriptionKey, Region);
                    Console.Write("\r\nPress Enter to return to the menu");
                    Console.ReadLine();

                    return true;
                case "2":
                    await SpeechRecognition.RecognizeSpeechContinuous(SubscriptionKey, Region);
                    Console.Write("\r\nPress Enter to return to the menu");
                    Console.ReadLine();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }
    }
}
