using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandCWD : CommandBase
    {
        public CommandCWD(FtpSession session)
            : base("CWD",session)
        {
            
        }
        protected override string OnProcessing(string message)
        {
            return "250 CWD successful.\r\n";
        }
    }
}
