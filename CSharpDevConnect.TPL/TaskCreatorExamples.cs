
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public sealed class TaskCreatorExamples
    {
        /// <summary>
        /// The task is instantiated using <c>new</c> and doesn't start until
        /// explicitly requested.
        /// </summary>
        [Fact]
        public void DelayedStartCreate()
        {
            ConsoleLogger.Log("Creating taskA");
            // Create a task and supply a delegate by using a lambda expression. 
            Task taskA = new Task(() => ConsoleLogger.Log("Hello from taskA."));

            ConsoleLogger.Log("Main thread doing some other work.");
            Thread.Sleep(100);

            // Start the task.
            ConsoleLogger.Log("Starting taskA");
            taskA.Start();

            ConsoleLogger.Log("Waiting for task to finish.");
            taskA.Wait();

            ConsoleLogger.Log("Finished.");
        }

        /// <summary>
        /// The task is created and started immediately by using the <see cref="Task.Factory"/>.
        /// </summary>
        [Fact]
        public void ImmediateStart()
        {
            ConsoleLogger.Log("Creating taskB");

            Task taskB = Task.Factory.StartNew(() => ConsoleLogger.Log("Hello from taskB"));

            ConsoleLogger.Log("Main thread doing some other work.");
            Thread.Sleep(100);

            ConsoleLogger.Log("Waiting for task to finish.");
            taskB.Wait();

            ConsoleLogger.Log("Finished.");
        }
    }
}