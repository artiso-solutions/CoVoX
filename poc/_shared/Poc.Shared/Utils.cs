using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public static class Utils
    {
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(() => tcs.SetResult());
            return tcs.Task;
        }

        public static CancellationToken GetUserCancellableToken(bool logInfo = true)
        {
            var cts = new CancellationTokenSource();

            _ = Task.Run(() =>
            {
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(intercept: true);
                } while (key.KeyChar != 'q' && key.KeyChar != 'Q');

                if (logInfo)
                    Console.WriteLine($"*** Quitting... ***");

                cts.Cancel();
            });

            if (logInfo)
            {
                Console.WriteLine();
                Console.WriteLine($"*** Press 'Q' to quit ***");
            }

            return cts.Token;
        }
    }
}
