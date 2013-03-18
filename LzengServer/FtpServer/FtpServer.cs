using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Configuration;
using System.Net;

namespace LzengServer.FtpServer
{
    class FtpServer
    {
        readonly TcpListener ftpListener;
        IDictionary<string,FtpSession> sessions = new Dictionary<string,FtpSession>(StringComparer.CurrentCulture);
        public FtpServer()
        {
            ftpListener = new TcpListener(IPAddress.Any, FtpSettings.FtpPort);
        }
        public async void Start()
        {
            ftpListener.Start(100);
            TcpClient tcpClient = null;
            tcpClient = await ftpListener.AcceptTcpClientAsync();
            while (tcpClient != null)
            {
                var session = new FtpSession(tcpClient);
                session.SessionClosed += (id) => sessions.Remove(id);
                sessions.Add(session.Id, session);
                tcpClient = await ftpListener.AcceptTcpClientAsync();
            }
        }
        public void Stop()
        {
            ftpListener.Stop();
        }
        public static IPAddress LocalIPAddress
        {
            get
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                if (hostEntry == null || hostEntry.AddressList.Length == 0)
                {
                    return IPAddress.Any;
                }
                return hostEntry.AddressList[0];
            }            
        }
    }
}
