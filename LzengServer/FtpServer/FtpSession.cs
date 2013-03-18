using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace LzengServer.FtpServer
{
    enum FtpMode
    {
        None,
        Pasv,
        Port
    }
    class FtpSession : IDisposable
    {
        internal FtpMode mode { get; set; }
        readonly NetworkStream socketStream;
        readonly StreamWriter writer;
        readonly StreamReader reader;
        internal string Id { get; private set; }
        internal event Action<string> SessionClosed;
        const string WelcomeString = "220 FTP Server Ready\r\n";
        IDictionary<string, CommandBase> commands = new Dictionary<string, CommandBase>();
        internal FtpUser ftpUser;
        internal IPEndPoint PortIpEP { get; set; }
        internal TcpListener PasvTcpListener { get; set; }
        internal FtpSession(TcpClient client)
        {
            client.NoDelay = false;
            socketStream = client.GetStream();
            mode = FtpMode.None;
            writer = new StreamWriter(socketStream, Encoding.ASCII);
            reader = new StreamReader(socketStream, Encoding.ASCII);
            Id = Guid.NewGuid().ToString();
            ftpUser = new FtpUser(client.Client.RemoteEndPoint as IPEndPoint);
            LoadCommand();
            SendBackMessage(WelcomeString);
            Start();
        }
        async void Start()
        {
            var message = await reader.ReadToEndAsync();
            Start();
            ProcessCommand(message);
        }
        void LoadCommand()
        {

        }
        void ProcessCommand(string message)
        {
            message = message.Substring(0, message.IndexOf('\r'));

            string cmd = string.Empty;
            string value = string.Empty;

            var splitMsg = message.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            cmd = splitMsg[0].ToUpper();
            if (splitMsg.Length >= 2)
                value = splitMsg[1];

            if (commands.ContainsKey(cmd))
            {
                var command = commands[cmd];
                command.Process(value);
            } 
            else
            {
                SendBackMessage("550 Unknown command\r\n");
            }
        }
        internal void SendBackMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;
        }
        internal void SendBackMessage(byte[] msg)
        {
            if (msg == null || msg.Length <= 0)
                return;
            socketStream.Write(msg, 0, msg.Length);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
