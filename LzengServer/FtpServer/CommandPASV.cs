using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandPASV : CommandBase
    {
        public CommandPASV(FtpSession session)
            : base("PASV", session)
        {

        }
        protected override string OnProcessing(string message)
        {
            for (int port = FtpSettings.PasvMinPort; port <= FtpSettings.PasvMaxPort;port++ )
            {
                var tcpListener = new TcpListener(IPAddress.Any, port);
                try
                {
                    tcpListener.Start();
                    var localIPAddr = FtpServer.LocalIPAddress.ToString().Replace('.', ',');
                    localIPAddr += string.Format(",0,{0}", port);
                    ftpSession.mode = FtpMode.Pasv;
                    return ("227 Entering Passive Mode (" + localIPAddr + ").\r\n");
                }
                catch (System.Exception ex)
                {
                    
                }
            }
            return "500 Pasv Failed \r\n";         
        }
    }
}
