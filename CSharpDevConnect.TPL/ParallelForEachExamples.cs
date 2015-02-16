using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public sealed class ParallelForEachExamples
    {
        [Fact]
        public void ParallelForEachWithParallelOptions()
        {
            Console.WriteLine("Thread\tMessage");

            string[] messages = new[] { "Hi", "How ya doin' b'y?", "Bye", "This pizza is yummy!" };

            ParallelOptions options = new ParallelOptions
                                          {
                                              MaxDegreeOfParallelism = 4,  // Maximum number of taks that can be run concurrently
                                              CancellationToken = new CancellationToken()
                                          };

            Parallel.ForEach(messages, options, ProcessMessage);

        }

        private void ProcessMessage(string message)
        {
            ConsoleLogger.Log(message);
        }
    }
}