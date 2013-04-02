using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer
{
    interface IServer
    {
        void Start();
        void Stop();
    }
    interface ISession
    {
        string SessionId { get; }
        event Action<ISession> Closed;
    }
}
