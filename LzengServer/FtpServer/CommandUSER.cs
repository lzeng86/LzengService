using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandUSER : CommandBase
    {
        public CommandUSER(FtpSession session)
            : base("USER",session)
        {
            
        }
        protected override string OnProcessing(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return "503 Invalid User Name\r\n";
            }
            else
            {
                ftpSession.ftpUser.User = message;
                return "331 Password required!\r\n";
            }
        }
    }
}
