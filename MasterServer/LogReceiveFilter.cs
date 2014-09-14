using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.MasterServer
{
    public class LogReceiveFilter : IReceiveFilter<StringPackageInfo>
    {
        public StringPackageInfo Filter(BufferList data, out int rest)
        {
            rest = 0;

            using (var reader = this.GetBufferReader(data))
            {
                return new StringPackageInfo(string.Empty, reader.ReadString(data.Total, Encoding.UTF8), null);
            }
        }

        public IReceiveFilter<StringPackageInfo> NextReceiveFilter { get; private set; }

        public void Reset()
        {

        }

        public FilterState State { get; private set; }
    }
}
