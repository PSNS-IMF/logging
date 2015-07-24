using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Text;

using System.Threading.Tasks;

using Psns.Common.Logging;
using Psns.Common.Test.BehaviorDrivenDevelopment;

namespace Logging.UnitTests
{
    [TestClass]
    public class WhenWritingToALogger
    {
        [TestInitialize]
        public void Initialize()
        {
            File.Delete("TestLog.txt");
        }

        [TestMethod]
        public void ThenAnEntryShouldBeWrittenToTheSpecifiedLogWithTheCorrectFormatting()
        {
            var log = new Logger();
            log.Write(new LoggerEntry("Test Category")
            {
                Message = "Test Entry",
                UserName = "User One",
                Priority = 1,
                Severity = System.Diagnostics.TraceEventType.Error,
                EventId = 44
            });
            log.Dispose();

            var path = "TestLog.txt";
            Assert.IsTrue(File.Exists(path));

            using(var reader = new StreamReader(path))
            {
                var text = reader.ReadLine();
                Assert.IsTrue(text.EndsWith("Error\tUser One\tTest Category\t1\tTest Entry\t44"));
                Assert.IsTrue(text.StartsWith(DateTime.Today.ToShortDateString()));
            }
        }
    }

    [TestClass]
    public class WhenWritingToALoggerWithNoUserName
    {
        [TestInitialize]
        public void Initialize()
        {
            File.Delete("TestLog.txt");
        }

        [TestMethod]
        public void ThenAnEntryShouldBeWrittenToTheSpecifiedLogWithTheCorrectFormatting()
        {
            var log = new Logger();
            log.Write(new LoggerEntry("Test Category")
            {
                Message = "Test Entry",
                Priority = 1,
                Severity = System.Diagnostics.TraceEventType.Error,
                EventId = 44
            });
            log.Dispose();

            var path = "TestLog.txt";
            Assert.IsTrue(File.Exists(path));

            using(var reader = new StreamReader(path))
            {
                var text = reader.ReadLine();
                Assert.IsTrue(text.EndsWith("Error\t\tTest Category\t1\tTest Entry\t44"));
                Assert.IsTrue(text.StartsWith(DateTime.Today.ToShortDateString()));
            }
        }
    }

    [TestClass]
    public class WhenWritingToALoggerFromMultipleThreadsWithACollectionBeingModifiedDuringExecution : BehaviorDrivenDevelopmentBase
    {
        Logger _logger;
        string _logFilePath;
        Task[] _tasks;

        const int _threadCount = 100;

        public override void Arrange()
        {
            base.Arrange();

            _logFilePath = "TestLog.txt";

            File.Delete(_logFilePath);

            _logger = new Logger();

            var collectionToLog = new List<string>();
            for(int i = 0; i < 100; i++)
            {
                collectionToLog.Add(i.ToString());
            }

            _tasks = new Task[_threadCount+1];

            _tasks[50] = new Task(() =>
                {
                    collectionToLog.RemoveAt(50);
                });

            for(int i = 0; i < _threadCount+1; i++)
            {
                if(i == 50) continue;

                _tasks[i] = new Task(() =>
                    {
                        _logger.Write(new LoggerEntry("Test Category")
                        {
                            Message = new Func<string>(() =>
                            {
                                var messageBuilder = new StringBuilder();

                                for(int j = 0; j < collectionToLog.Count; j++)
                                {
                                    messageBuilder.Append(collectionToLog[j]);

                                    if(j != collectionToLog.Count - 1)
                                        messageBuilder.Append(" ");
                                }

                                return messageBuilder.ToString();
                            }).Invoke(),
                            UserName = "User One",
                            Priority = 1,
                            Severity = System.Diagnostics.TraceEventType.Error,
                            EventId = 44
                        });
                    });
            }
        }

        public override void Act()
        {
            base.Act();

            foreach(var task in _tasks)
            {
                task.Start();
            }
        }

        [TestMethod]
        public void ThenTheWritingShouldSucceed()
        {
            Task.WaitAll(_tasks);
            _logger.Dispose();

            Assert.IsTrue(File.Exists(_logFilePath));

            using(var reader = new StreamReader(_logFilePath))
            {
                var text = reader.ReadLine();
                Assert.IsTrue(text.EndsWith("\t44"));
                Assert.IsTrue(text.StartsWith(DateTime.Now.ToShortDateString()));
            }
        }
    }
}
