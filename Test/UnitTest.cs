using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using LogMaster4Net.Base;
using LogMaster4Net.Log4NetAdapter;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public abstract class UnitTestBase
    {
        protected abstract ILoggingDeserializer GetLoggingDeserializer();

        protected abstract string GetAssertSubDir();

        [Test]
        public void TestDeserializeSingleLog()
        {
            ILoggingDeserializer deserializer = GetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", GetAssertSubDir(), "SingleLog.xml")));
            
            Assert.AreEqual(1, logs.Count);

            var log = logs[0];

            Assert.AreEqual("LogMaster", log.LoggerName);
            Assert.AreEqual("Hello World", log.Message);
        }

        [Test]
        public void TestDeserializeExceptionLog()
        {
            ILoggingDeserializer deserializer = GetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", GetAssertSubDir(), "ExceptionLog.xml")));
            Assert.AreEqual(3, logs.Count);
            Assert.IsNotNullOrEmpty(logs[0].ExceptionString);
        }

        [Test]
        public void TestDeserializeMultipleLog()
        {
            ILoggingDeserializer deserializer = GetLoggingDeserializer();
            var logs = deserializer.Deserialize(File.ReadAllText(Path.Combine("Asserts", GetAssertSubDir(), "MultipleLog.xml")));
            Assert.AreEqual(2, logs.Count);
            Assert.IsNotNullOrEmpty(logs[0].Message);
        }
    }
}
