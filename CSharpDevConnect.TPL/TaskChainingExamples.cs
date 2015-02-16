using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public sealed class TaskChainingExamples
    {
        [Fact]
        public void ChainOnSuccess()
        {
            // Specify that the secondary task should only run if the first completes successfully
            const TaskContinuationOptions TASK_CONTINUATION_OPTIONS = TaskContinuationOptions.OnlyOnRanToCompletion;

            Task task1 = new Task(() =>
                {
                    ConsoleLogger.Log("Running task1");
                    Thread.Sleep(100);
                });

            // Schedule the second task to run after task1 completes successfully
            Task task2 = task1.ContinueWith((t) => ConsoleLogger.Log("Running task2"), TASK_CONTINUATION_OPTIONS);

            task1.Start();

            task2.Wait();

            ConsoleLogger.Log(string.Format("task2 finished with status={0}", task2.Status));
        }

        [Fact]
        public void ChainOnSuccessFirstTaskFaults()
        {
            // Specify that the secondary task should only run if the first completes successfully
            const TaskContinuationOptions TASK_CONTINUATION_OPTIONS = TaskContinuationOptions.OnlyOnRanToCompletion;

            Task task1 = new Task(() =>
            {
                ConsoleLogger.Log("Running task1");
                Thread.Sleep(100);
                throw new Exception("First task failed.");
            });

            // Schedule the second task to run after task1 completes successfully
            Task task2 = task1.ContinueWith((t) => ConsoleLogger.Log("Running task2"), TASK_CONTINUATION_OPTIONS);

            try
            {
                task1.Start();

                task2.Wait();  // This will fail with a TaskCanceledException
            }
            catch (AggregateException e)
            {
                ConsoleLogger.Log(e.Message);
            }

            ConsoleLogger.Log(string.Format("task2 finished with status={0}", task2.Status));
        }
    }
}