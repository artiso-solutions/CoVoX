using System;

namespace Shared
{
    public abstract class AbstractScenario
    {
        public string SubscriptionKey { get; }

        public string Region { get; }

        public AbstractScenario(
            string subscriptionKey,
            string region,
            bool logScenarioName = true)
        {
            SubscriptionKey = subscriptionKey;
            Region = region;

            if (logScenarioName)
            {
                var scenarioName = GetType().Name.Replace("Scenario", "");
                Console.WriteLine();
                Console.WriteLine($"// Scenario: {scenarioName}");
            }
        }
    }
}
