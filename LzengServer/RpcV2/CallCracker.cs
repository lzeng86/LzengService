using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.RpcV2
{
    class CallCracker
    {
        int ind = 0;
        readonly byte[] data;
        public CallCracker(byte[] data)
        {
            this.data = data;
        }
        public int PopInt32()
        {
            var value = BitConverter.ToInt32(data, ind);
            ind += 4;
            return value;
        }

        public string PopString()
        {
            var len = PopInt32();
            var value = BitConverter.ToString(data, ind, len);
            ind += len;
            Alignment();
            return value;
        }

        public byte[] PopData()
        {
            var len = PopInt32();
            byte[] value = new byte[len];
            Buffer.BlockCopy(data, ind, value, 0, len);
            ind += len;
            Alignment();
            return value;
        }

        void Alignment()
        {
            if (ind % 4 != 0)
                ind += (4 - ind % 4);
        }
    }
}
