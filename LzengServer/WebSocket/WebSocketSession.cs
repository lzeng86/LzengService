using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.WebSocket
{
    enum WrapperBytes : byte //由WebSOcket协议规定，消息开始于0x00，结束于0xFF
    {
        Start = 0, 
        End = 255 
    }
    class WebSocketSession : ISession
    {
        byte[] buffer = new byte[10];
        readonly Socket connSocket;
        StringBuilder receiveData = new StringBuilder();
        public WebSocketSession(Socket conn)
        {
            SessionId = Guid.NewGuid().ToString();            
            connSocket = conn;
            connSocket.BeginReceive(buffer, 0, 2, SocketFlags.None, EndReceive, null);
        }
        public string SessionId{get;private set;}

        public event Action<ISession> Closed;

        int FIN = 1 ,RSV1 = 0, RSV2 = 0, RSV3 = 0;

        void EndReceive(IAsyncResult ar)
        {
            var size = connSocket.EndReceive(ar);
            if (size == 0 || size != 2)
                return;
            if (FIN == 1)
            {
                NewFragment();
            } 
            else
            {
            }
            
        }

        void NewFragment()
        {
            FIN = buffer[0] >> 7 & 0x01;
            RSV1 = buffer[0] >> 6 & 0x01;
            RSV2 = buffer[0] >> 5 & 0x01;
            RSV3 = buffer[0] >> 4 & 0x01;
            var opCode = buffer[0] & 0x0F;
            var mask = buffer[1] >> 7;
            var payloadLen = (buffer[1] & 0x7F);
            if (payloadLen == 126)
            {
                connSocket.Receive(buffer, 2, SocketFlags.None);
                payloadLen = buffer[0] * 256 + buffer[1];
            }
            else if (payloadLen == 127)
            {
                connSocket.Receive(buffer, 8, SocketFlags.None);
                payloadLen = 0;
                for (int i = 0; i < 3; i++)
                {
                    payloadLen *= 256;
                    payloadLen += buffer[i];
                }
            }
            Console.WriteLine("len -> " + payloadLen);
            var maskKey = new byte[4];
            if (mask == 1)
            {
                connSocket.Receive(maskKey, 4, SocketFlags.None);
            }
            var data = new byte[payloadLen];
            connSocket.Receive(data, data.Length, SocketFlags.None);
            if (mask == 1)
            {
                for (int i = 0; i < (int)payloadLen; i++)
                {
                    data[i] ^= maskKey[i & 0x3];
                }
            }
            var s = Encoding.UTF8.GetString(data);
            Console.WriteLine(s);
            connSocket.BeginReceive(buffer, 0, 2, SocketFlags.None, EndReceive, null);
        }

        void RemainFragment()
        {

        }
    }
}
