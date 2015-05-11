using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMaster4Net.Base;
using LogMaster4Net.Log4NetAdapter;

namespace Test
{
    public class Log4NetTest : UnitTestBase
    {
        protected override ILoggingDeserializer GetLoggingDeserializer()
        {
            return new Log4NetLoggingDeserializer();
        }

        protected override string GetAssertSubDir()
        {
            return "log4net";
        }
    }
}
