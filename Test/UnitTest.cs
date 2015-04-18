using log4net.Core;
using LogMaster4Net.Base;
using LogMaster4Net.Log4NetAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void TestDeserializeSingleLog()
        {
            ILoggingDeserializer deserializer = new Log4NetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", "SingleLog.xml")));
            Assert.AreEqual(1, logs.Count);
        }

        [Test]
        public void TestDeserializeExceptionLog()
        {
            ILoggingDeserializer deserializer = new Log4NetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", "ExceptionLog.xml")));
            Assert.AreEqual(1, logs.Count);
            Assert.IsNotNullOrEmpty(logs[0].ExceptionString);
        }

        [Test]
        public void TestDeserializeMultipleLog()
        {
            ILoggingDeserializer deserializer = new Log4NetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", "MultipleLog.xml")));
            Assert.AreEqual(1, logs.Count);
            Assert.IsNotNullOrEmpty(logs[0].ExceptionString);
        }
    }
}
