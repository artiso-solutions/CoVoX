using System.Threading.Tasks;

namespace TextTranslate
{
    class Program
    {
        private static readonly string SubscriptionKey = "";
        private static readonly string Region = "";

        static async Task Main()
        {
            await RunApiDemoScenario();
        }

        public static async Task RunApiDemoScenario() =>
            await new ApiDemoScenario(SubscriptionKey, Region).Run();
    }
}
