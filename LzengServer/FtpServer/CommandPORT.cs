using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandPORT : CommandBase
    {
        public CommandPORT(FtpSession session)
            : base("PORT", session)
        {

        }
        protected override string OnProcessing(string message)
        {
            string[] ipParts = message.Split(new char[] { ',' });
            if (ipParts.Length != 6)
            {
                ftpSession.mode = FtpMode.None;
                return "550 Arguments error.";
            }
            int port = int.Parse(ipParts[4]) << 8 + int.Parse(ipParts[5]);
            var addr = IPAddress.Parse(string.Join(".", ipParts, 0, 4));
            ftpSession.PortIpEP = new IPEndPoint(addr , port);
            ftpSession.mode = FtpMode.Port;
            return "200 Command succeeded";
        }
    }
}
