using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace LzengServer.FtpServer
{
    class FtpDataConnection : IDisposable
    {
        TcpClient dataConnection;
        public FtpDataConnection(FtpSession session)
        {            
            dataConnection = GetPasvConnection(session);
            if (dataConnection == null)
                dataConnection = GetPortConnection(session);
        }
        TcpClient GetPasvConnection(FtpSession session)
        {
            var task = new Task<TcpClient>(() => 
                {
                    try
                    {
                        return session.PasvTcpListener.AcceptTcpClient();
                    }
                    catch
                    {
                        return null;
                    }                    
                }                
            );
            task.Wait(FtpSettings.PasvTimeout);
            if (task.IsCompleted)
                return task.Result;
            return null;
        }
        TcpClient GetPortConnection(FtpSession session)
        {
            var task = new Task<TcpClient>(() =>
            {
                try
                {
                    var localIPEP = new IPEndPoint(FtpServer.LocalIPAddress,20);
                    var client = new TcpClient(localIPEP);
                    client.Connect(session.PortIpEP);
                    return client;
                }
                catch
                {
                    return null;
                }
            }
            );
            task.Wait(FtpSettings.PortTimeout);
            if (task.IsCompleted)
                return task.Result;
            return null;
        }
        public void Close()
        {
            if (dataConnection != null)
            {
                dataConnection.Close();
            }
        }
        public void Dispose()
        {
            Close();
        }
        public void Send(string message)
        {
            if (dataConnection == null)
                throw new InvalidOperationException();
            var data = Encoding.Default.GetBytes(message);
            dataConnection.GetStream().Write(data, 0, data.Length);
        }
        public string Receive()
        {
            if (dataConnection == null)
                throw new InvalidOperationException();
            var buffer = new byte[100];
            using (var ms = new MemoryStream())
            {
                do 
                {
                    var size = dataConnection.GetStream().Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, size);
                } while (dataConnection.GetStream().DataAvailable);
                return Encoding.Default.GetString(ms.ToArray());
            }            
        }
    }
}
