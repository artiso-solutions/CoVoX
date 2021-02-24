using System.Threading.Tasks;

namespace TextTranslate
{
    class Program
    {
        private static readonly string SubscriptionKey = "";
        private static readonly string Region = "";

        static async Task Main()
        {
            //await RunApiDemoScenario();
            await RunApiPerfScenario();
        }

        public static async Task RunApiDemoScenario() =>
            await new ApiDemoScenario(SubscriptionKey, Region).Run();

        public static async Task RunApiPerfScenario() =>
            await new ApiPerfScenario(SubscriptionKey, Region).Run();
    }
}
