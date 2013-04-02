using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.RpcV2
{
    class RpcMsgBuilder
    {
        List<byte> data = new List<byte>(1024);
        public void PushInt32(int value)
        {
            data.AddRange(BitConverter.GetBytes(value));
        }
        public void PushString(string value)
        {
            PushInt32(value.Length);
            data.AddRange(value.ToCharArray().Select(o=>(byte)o));
            Alignment();
        }
        public void PushData(byte[] value)
        {
            PushInt32(value.Length);
            data.AddRange(value);
            Alignment();
        }
        void Alignment()
        {
            for (var re = data.Count % 4; re != 0 && re < 4; re++)
            {
                data.Add((byte)0);
            }
        }
    }
}
