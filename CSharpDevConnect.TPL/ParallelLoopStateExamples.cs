using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public class ParallelLoopStateExamples
    {
        [Fact]
        public void StopOnByeMessage()
        {
            Console.WriteLine("Thread\tMessage");
            string[] messages = new[] { "Hi", "How ya doin' b'y?", "Bye", "This pizza is yummy!" };

            Parallel.ForEach(messages, ProcessMessage);
        }

        private static void ProcessMessage(string message, ParallelLoopState loopState)
        {
            if (message.Equals("Bye"))
            {
                Console.WriteLine("{0}\t--> Stopping on message: {1}", Thread.CurrentThread.ManagedThreadId, message);
                loopState.Stop();
                return;
            }

            if (!loopState.IsStopped)
            {
                Console.WriteLine("{0}\t{1}", Thread.CurrentThread.ManagedThreadId, message);
            }
            else
            {
                Console.WriteLine("{0}\t--> Skipping message: {1}", Thread.CurrentThread.ManagedThreadId, message);
            }
        }


    }
}
