using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandPASS : CommandBase
    {
        public CommandPASS(FtpSession session)
            : base("PASS",session)
        {
            
        }
        protected override string OnProcessing(string message)
        {
            ftpSession.ftpUser.Password = message;
            if (ftpSession.ftpUser.Login())
            {
                return "230 Authentication Successful.\r\n";
            }
            else
            {
                return "530 Authentication Failed.\r\n"; 
            }
        }
    }
}
