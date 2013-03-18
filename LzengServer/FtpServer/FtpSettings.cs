using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    static class FtpSettings
    {
        public static int FtpPort
        {
            get
            {
                var portStr = ConfigurationManager.AppSettings["ftpPort"];
                int port;
                if (!Int32.TryParse(portStr, out port))
                    port = 21;
                return port;
            }
        }
        public static int PasvMinPort 
        { 
            get
            {
                var portStr = ConfigurationManager.AppSettings["pasvMinPort"];
                int port;
                if (!Int32.TryParse(portStr, out port))
                    port = 7000;
                return port;
            }
        }
        public static int PasvMaxPort
        {
            get
            {
                var portStr = ConfigurationManager.AppSettings["pasvMaxPort"];
                int port;
                if (!Int32.TryParse(portStr, out port))
                    port = 7100;
                return port;
            }
        }
        public static TimeSpan PasvTimeout
        {
            get
            {
                var timeoutStr = ConfigurationManager.AppSettings["pasvTimeout"];
                TimeSpan timeout;
                if (!TimeSpan.TryParse(timeoutStr, out timeout))
                    timeout = TimeSpan.FromSeconds(30.0);
                return timeout;
            }
        }
        public static TimeSpan PortTimeout
        {
            get
            {
                var timeoutStr = ConfigurationManager.AppSettings["portTimeout"];
                TimeSpan timeout;
                if (!TimeSpan.TryParse(timeoutStr, out timeout))
                    timeout = TimeSpan.FromSeconds(30.0);
                return timeout;
            }
        }
    }
}
