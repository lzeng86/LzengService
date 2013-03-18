using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandPWD : CommandBase
    {
        public CommandPWD(FtpSession session)
            : base("PWD",session)
        {
            
        }
        protected override string OnProcessing(string message)
        {
            return "257 \".\"\r\n";
        }
    }
}
