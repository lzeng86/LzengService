using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.FtpServer
{
    class FtpUser
    {
        IPAddress clientAddress;
        string validUser = string.Empty;
        string validPassword = string.Empty;
        public FtpUser(IPEndPoint ipEP)
        {
            clientAddress = ipEP.Address;
            User = string.Empty;
            Password = string.Empty;
        }
        void Initialize()
        {

        }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Login()
        {
            return (validUser == User && validPassword == Password);
        }
    }
}
