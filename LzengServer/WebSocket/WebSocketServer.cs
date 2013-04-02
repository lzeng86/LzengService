using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace LzengServer.WebSocket
{
    class WebSocketServer : IServer
    {
        Socket listener = null;
        IPEndPoint ipLocalEP;
        TextWriter Logger = Console.Out;
        public WebSocketServer(int port)
        {
            ipLocalEP = new IPEndPoint(IPAddress.Loopback, port);                        
        }
        public void Start()
        {
            Stop();
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listener.Bind(ipLocalEP);
            listener.Listen(100);
            listener.BeginAccept(EndReceive, null);
        }
        void EndReceive(IAsyncResult ar)
        {
            try
            {
                var socket = listener.EndAccept(ar);
                Logger.WriteLine("New connection");
                listener.BeginAccept(EndReceive, null);
                ShakeHands(socket);
                var session = new WebSocketSession(socket);
            }
            catch(Exception ex)
            {
                listener = null;
            }
        }
        void ShakeHands(Socket conn)
        {
            using (var stream = new NetworkStream(conn))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                //Logger.WriteLine("Receive:");
                var heads = new Dictionary<string, string>();
                while (true)
                {
                    var r = reader.ReadLine();
                    if (r == "")
                        break;
                    //Logger.WriteLine(r);
                    var ind = r.IndexOf(':');
                    if (ind > 0)
                    {
                        heads[r.Substring(0,ind).Trim()] = r.Substring(ind+1).Trim();
                    }                    
                }
                var key = heads["Sec-WebSocket-Key"];
                heads.Clear();
                key += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                byte[] result = new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(key));
                var re = Convert.ToBase64String(result);
                //Logger.WriteLine("re -> "+re);
                //var input = reader.ReadToEnd();
                //Logger.WriteLine(input);
                //// send handshake to the client
                writer.WriteLine("HTTP/1.1 101 Switching Protocols ");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                writer.WriteLine("Sec-WebSocket-Accept: " + re);
                //writer.WriteLine("WebSocket-Location: " + webSocketLocation);
                writer.WriteLine("");
            }


            //// tell the nerds whats going on
            //LogLine("Sending handshake:", ServerLogLevel.Verbose);
            //LogLine("HTTP/1.1 101 Web Socket Protocol Handshake", ServerLogLevel.Verbose);
            //LogLine("Upgrade: WebSocket", ServerLogLevel.Verbose);
            //LogLine("Connection: Upgrade", ServerLogLevel.Verbose);
            //LogLine("WebSocket-Origin: " + webSocketOrigin, ServerLogLevel.Verbose);
            //LogLine("WebSocket-Location: " + webSocketLocation, ServerLogLevel.Verbose);
            //LogLine("", ServerLogLevel.Verbose);

            //LogLine("Started listening to client", ServerLogLevel.Verbose);
            //conn.Listen();
        }
        public void Stop()
        {
            if (listener != null)
                listener.Close(1000);
            //listener = null;
        }
    }
}
