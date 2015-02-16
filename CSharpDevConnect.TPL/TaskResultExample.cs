using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Sdk;

namespace CSharpDevConnect.TPL.Examples
{
    public class TaskResultExample
    {
        [Fact]
        public void TaskWithResult()
        {
            Task<bool> boolTask = new Task<bool>(() => true);
            boolTask.Start();
            boolTask.Wait();

            ConsoleLogger.Log(string.Format("boolTask returned: {0}", boolTask.Result));
        }
    }
}