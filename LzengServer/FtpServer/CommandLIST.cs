using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class CommandLIST : CommandBase
    {
        public CommandLIST(FtpSession session)
            : base("LIST", session)
        {
            
        }
        protected override string OnProcessing(string message)
        {
            ftpSession.SendBackMessage("150 Connecting.\r\n");
            try
            {
                using (var dc = new FtpDataConnection(ftpSession))
                {
                    dc.Send("");
                }
                return "226 List successful.\r\n";
            }
            catch (InvalidOperationException)
            {
                return "425 Connection open failed.\r\n";
            }
            catch
            {
                return "426 Connection closed; transfer aborted.\r\n";
            }            
        }
    }
}
