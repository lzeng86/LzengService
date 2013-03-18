using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandBase
    {
        protected FtpSession ftpSession;
        public string Cmd { get; private set; }
        protected CommandBase(string cmd,FtpSession session)
        {
            ftpSession = session;
            Cmd = cmd;
        }
        protected virtual string OnProcessing(string message)
        {
            return string.Empty;
        }
        protected virtual string OnProcessed(string message)
        {
            return string.Empty;
        }
        public void Process(string message)
        {            
            ftpSession.SendBackMessage(OnProcessing(message));
            ftpSession.SendBackMessage(OnProcessed(message));
        }
    }
}
