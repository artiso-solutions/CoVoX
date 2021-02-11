using System.Threading.Tasks;

namespace SpeechTranslation
{
    class Program
    {
        static async Task Main()
        {
            var key = "SubscriptionKey";
            var region = "Region";

            // Languages: https://aka.ms/speech/sttt-languages

            await new RecognizeOnceScenario(key, region).Run(
                inputLanguage: "it-IT",
                targetLanguages: new[] { "en-US", "en-UK", "de-DE" });
        }
    }
}
