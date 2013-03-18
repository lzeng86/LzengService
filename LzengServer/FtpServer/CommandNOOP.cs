using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandNOOP : CommandBase
    {
        public CommandNOOP(FtpSession session)
            : base("NOOP", session)
        {

        }
        protected override string OnProcessing(string message)
        {
            return "200 \r\n";
        }
    }
}
