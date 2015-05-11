using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMaster4Net.Base;
using LogMaster4Net.NLogAdapter;

namespace Test
{
    public class NLogTest : UnitTestBase
    {
        protected override ILoggingDeserializer GetLoggingDeserializer()
        {
            return new NLogLoggingDeserializer();
        }

        protected override string GetAssertSubDir()
        {
            return "nlog";
        }
    }
}
