using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psns.Common.Logging;

namespace Logging.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();
            var tasks = new List<Task>();

            for(var i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => logger.Error($"test from thread { Task.CurrentId }")));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
