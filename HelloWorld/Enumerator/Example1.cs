using System;
using System.Collections.Generic;
using System.Collections.Async;
using System.Threading.Tasks;

namespace Enumerator
{
    public class ClassNormal
    {
        internal static IEnumerable<int> ProduceNumbers(int start, int end)
        {
            yield return start;

            for (int number = start + 1; number <= end; number++)
                yield return number;

            yield break;

            yield return 12345;
        }
        public static void ConsumeNumbers()
        {
            var enumerableCollection = ProduceNumbers(start: 1, end: 10);
            foreach (var number in enumerableCollection)
            {
                Console.Out.WriteLine($"{number}");
            }
        }        
    }

    public class ClassAsync
    {
        internal static IAsyncEnumerable<int> ProduceAsyncNumbers(int start, int end)
        {
            return new AsyncEnumerable<int>(async yield => {

                // Just to show that ReturnAsync can be used multiple times
                await yield.ReturnAsync(start);

                for (int number = start + 1; number <= end; number++)
                    await yield.ReturnAsync(number);

                // You can break the enumeration loop with the following call:
                yield.Break();

                // This won't be executed due to the loop break above
                await yield.ReturnAsync(12345);
            });
        }
        public static async Task ConsumeNumbersAsync()
        {
            var asyncEnumerableCollection = ProduceAsyncNumbers(start: 1, end: 10);
            await asyncEnumerableCollection.ForEachAsync(async number => {
                await Console.Out.WriteLineAsync($"{number}");
            });
        }
    }
}
