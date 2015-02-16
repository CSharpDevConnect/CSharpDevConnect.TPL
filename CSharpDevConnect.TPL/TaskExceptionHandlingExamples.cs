
using System;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public class TaskExceptionHandlingExamples
    {
        [Fact]
        public void SimpleAggregateExceptionExample()
        {
            Task task = new Task(() =>
                {
                    throw new Exception("Example exception.");
                });

            try
            {
                task.Start();
                task.Wait(500);
            }
            catch (AggregateException ae)
            {
                // Iterate over all of the inner exceptions 
                foreach (Exception innerException in ae.InnerExceptions)
                {
                    ConsoleLogger.Log(string.Format("Caught {0}.", innerException));
                }
            }
        }

        [Fact]
        public void MultipleAggregateExceptionExample()
        {
            Task task = new Task(() =>
            {
                throw new Exception("Example exception.");
            });

            Task task2 = new Task(() =>
            {
                throw new Exception("Another example exception.");
            });

            try
            {
                Task[] tasks = { task, task2 };
                task.Start();
                task2.Start();

                Task.WaitAll(tasks, 500);  // WaitAll() waits for all the tasks in the array to stop
            }
            catch (AggregateException ae)
            {
                foreach (Exception innerException in ae.InnerExceptions)
                {
                    Console.WriteLine();
                    ConsoleLogger.Log(string.Format("Caught {0}.", innerException));

                }
            }
        }

        [Fact]
        public void HandlingExceptionFromTaskExceptionPropertyExample()
        {
            Task task1 = new Task(() =>
            {
                throw new Exception("Task1 faulted.");
            });

            // Add error handling child task
            Task errorHandlerTask = task1.ContinueWith((t) =>
            {
                if (t.IsFaulted && t.Exception != null)
                {
                    t.Exception.Handle((e) =>
                    {
                        ConsoleLogger.Log(string.Format("I have handled a {0}", e.GetType().Name));

                        // mark the exception as handled, 
                        // if the exception is not handled than an AggregateException will be 
                        // thrown when Wait() or Result is called on errorHandlerTask
                        return true;
                    });
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                task1.Start();

                errorHandlerTask.Wait(500);
            }
            catch (AggregateException ae)
            {
                ConsoleLogger.Log("AggregateException caught on wait.");
            }
        }

    }
}