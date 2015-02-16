using System;
using System.Threading;

namespace CSharpDevConnect.TPL.Examples
{
    public sealed class ConsoleLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine("{0}\t{1}", Thread.CurrentThread.ManagedThreadId, message);
        }
    }
}