using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;

using Psns.Common.Logging;

namespace Logging.UnitTests
{
    [TestClass]
    public class WhenInstantiatingALoggerEntry
    {
        [TestMethod]
        public void ThenDefaultValuesShouldBeSetAsExpected()
        {
            var entry = new LoggerEntry("category");

            Assert.AreEqual<string>(string.Empty, entry.UserName);
            Assert.AreEqual<string>(string.Empty, entry.Message);
            Assert.AreEqual<string>("category", entry.Category);
            Assert.AreEqual<TraceEventType>(TraceEventType.Information, entry.Severity);
        }
    }

    [TestClass]
    public class WhenInstantiatingALoggerEntryWithANullCategory
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenANullExceptionShouldBeThrown()
        {
            var entry = new LoggerEntry(null);

            Assert.Fail();
        }
    }
}
