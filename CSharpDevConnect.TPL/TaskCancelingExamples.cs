
using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace CSharpDevConnect.TPL.Examples
{
    public sealed class TaskCancelingExamples
    {
        [Fact]
        public void GracefulCancelExample()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            TaskIterationCanceller iterationCanceller = new TaskIterationCanceller(5, tokenSource);

            Task taskToCancel = new Task(() =>
                {
                    while (!tokenSource.IsCancellationRequested)
                    {
                        iterationCanceller.CheckForCancelIteration();
                        Thread.Sleep(100);
                    }
                });

            taskToCancel.Start();
            taskToCancel.Wait();

            // Final status will be RanToCompletion
            ConsoleLogger.Log(string.Format("Task final state: {0}", taskToCancel.Status));
        }

        [Fact]
        public void ControlledCancelExceptionExample()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            TaskIterationCanceller iterationCanceller = new TaskIterationCanceller(5, tokenSource);

            Task taskToCancel = new Task(() =>
            {
                while (true)
                {
                    if (tokenSource.IsCancellationRequested)
                    {
                        // Do any needed clean up here.

                        tokenSource.Token.ThrowIfCancellationRequested();
                    }

                    iterationCanceller.CheckForCancelIteration();
                    Thread.Sleep(100);
                }
            }, tokenSource.Token);

            taskToCancel.Start();

            try
            {
                taskToCancel.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (Exception innerException in ae.InnerExceptions)
                {
                    if (innerException is TaskCanceledException)
                    {
                        ConsoleLogger.Log("Task Was Cancelled.");
                    }
                    else
                    {
                        throw innerException;
                    }
                }
            }

            // Final status will be Canceled
            ConsoleLogger.Log(string.Format("Task final state: {0}", taskToCancel.Status));
        }

        [Fact]
        public void HardCancelInWaitExample()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            TaskIterationCanceller iterationCanceller = new TaskIterationCanceller(5, tokenSource);

            Task taskToCancel = new Task(() =>
            {
                while (true)
                {
                    iterationCanceller.CheckForCancelIteration();
                    Thread.Sleep(100);
                }
            });

            taskToCancel.Start();

            try
            {
                taskToCancel.Wait(tokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                ConsoleLogger.Log(string.Format("Caught OperationCanceledException: {0}", e));
            }

            // Final status will be Running
            ConsoleLogger.Log(string.Format("Task final state: {0}", taskToCancel.Status));
        }

        [Fact]
        public void HardCancelInTaskConstructorExample()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            TaskIterationCanceller iterationCanceller = new TaskIterationCanceller(5, tokenSource);

            // By passing the token in the constructor, it will prevent the task from being allocated 
            // if the token source is cancelled before it is scheduled/started
            Task taskToCancel = new Task(() =>
            {
                while (true)
                {
                    iterationCanceller.CheckForCancelIteration();

                    Thread.Sleep(100);
                }
            }, tokenSource.Token);

            tokenSource.Cancel();

            try
            {
                // Because the token has already been cancelled, this will throw an InvalidOperationException
                taskToCancel.Start();

                taskToCancel.Wait();
            }
            catch (InvalidOperationException e)
            {
                ConsoleLogger.Log(string.Format("Caught InvalidOperationException: {0}", e));
            }

            // Final status will be Canceled
            ConsoleLogger.Log(string.Format("Task final state: {0}", taskToCancel.Status));
        }


        /// <summary>
        /// A simple helper class that will cancel a <see cref="CancellationTokenSource"/> after
        /// a set number of calls
        /// </summary>
        class TaskIterationCanceller
        {
            private readonly int _cancelIteration;

            private int _currentIteration = 0;

            private readonly CancellationTokenSource _cancellationTokenSource;

            /// <summary>
            /// Initializes a new instance of the <see cref="TaskIterationCanceller"/> class.
            /// </summary>
            /// <param name="cancelIteration">The number of iterations to wait before calling Cancel() on <paramref name="cancellationTokenSource"/></param>
            /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource"/> to act upon.</param>
            public TaskIterationCanceller(int cancelIteration, CancellationTokenSource cancellationTokenSource)
            {
                _cancelIteration = cancelIteration;
                _cancellationTokenSource = cancellationTokenSource;
            }

            /// <summary>
            /// Checks to see if the token source should be cancelled.
            /// </summary>
            /// <returns>True, if the token source has been cancelled.</returns>
            public bool CheckForCancelIteration()
            {
                _currentIteration++;

                if (_currentIteration >= _cancelIteration)
                {
                    ConsoleLogger.Log(string.Format("Cancelling task after {0} iterations", _currentIteration));
                    _cancellationTokenSource.Cancel();
                }

                return _cancellationTokenSource.IsCancellationRequested;
            }
        }
    }
}