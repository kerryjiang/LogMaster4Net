using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LogMaster4Net.Base
{
    public struct LocationInfo
    {
        public string ClassName { get; set; }
        public string FileName { get; set; }
        public string LineNumber { get; set; }
        public string MethodName { get; set; }
        public string FullInfo { get; set; }
        public StackFrame[] StackFrames { get; set; }
    }
}
