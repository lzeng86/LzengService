using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandQUIT : CommandBase
    {
        public CommandQUIT(FtpSession session)
            : base("QUIT", session)
        {

        }
        protected override string OnProcessing(string message)
        {
            return "220 Thanks\r\n";
        }
    }
}
