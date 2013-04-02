using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace LzengServer.RpcV2
{
    abstract class RpcV2
    {
        UdpClient rpcUdp;
        public void Start(int port)
        {
            if (rpcUdp != null)
                return;
            rpcUdp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            Receive();
        }
        public void Stop()
        {
            if (rpcUdp != null)
                rpcUdp.Close();
            rpcUdp = null;
        }
        protected abstract string Id { get; }
        protected abstract void CallProcedure(int procId, RpcMsgCracker cracker, RpcMsgBuilder reply);
        protected abstract bool CheckProg(int progId, int progVers);
        protected virtual void Authenticate(RpcMsgCracker cracker)
        {

        }
        //void CrackCredentials(rpcCracker cracker)
        //{
        //    uint flavor = cracker.get_uint32();
        //    uint length = cracker.get_uint32();

        //    //Console.WriteLine("{0}: Credentials.  flavor:{1}, length:{2}", prog, flavor, length);

        //    cracker.jump(length);
        //}

        //void CrackVerifier(rpcCracker cracker)
        //{
        //    uint flavor = cracker.get_uint32();
        //    uint length = cracker.get_uint32();

        //    //Console.WriteLine("{0}: Credentials.  flavor:{1}, length:{2}", prog, flavor, length);

        //    cracker.jump(length);
        //}
        protected virtual void ServerCredential(RpcMsgBuilder reply)
        {
            reply.PushInt32((int)auth_flavor.AUTH_NONE);		// rpc_msg.reply_body.accepted_reply.opaque_auth.NULL
            reply.PushInt32(0);		// rpc_msg.reply_body.accepted_reply.opaque_auth.<datsize>
        }
        async public void Receive()
        {
            var receivedResult = await rpcUdp.ReceiveAsync();
            if (receivedResult == null)
                return;
            Receive();
            ProcessMsg(receivedResult);
        }
        RpcMsgBuilder ProcessMsg(UdpReceiveResult result)
        {
            var cracker = new RpcMsgCracker(result.Buffer);

            var xid = cracker.PopInt32();
            var msgType = (msg_type)cracker.PopInt32();    
            if (msgType != msg_type.CALL)
                return AcceptReply(xid,accept_stat.GARBAGE_ARGS);

            var rpcVers = cracker.PopInt32();
            if (rpcVers != 2)
                return RpcMismatchReply(xid);

            var progId = cracker.PopInt32();
            var progVers = cracker.PopInt32();
            var procId = cracker.PopInt32();
            if (!CheckProg(progId, progVers))
                return ProgMismatchReply(xid, progId);

            Authenticate(cracker);

            try
            {
                var reply = AcceptReply(xid,accept_stat.SUCCESS);
                //RPC中，编号为0的例程是一个标准例程，这个例程的作用是测试客户端和服务器端的连接是否正常，这个例程没有处理函数，也不需要认证
                if (procId == 0)    
                    return reply;
                CallProcedure(procId, cracker, reply);
                return reply;
            }
            catch (Exception e)
            {
                return AcceptReply(xid,accept_stat.PROC_UNAVAIL);
            }
        }

        RpcMsgBuilder AcceptReply(int xid,accept_stat reason)
        {
            var reply = new RpcMsgBuilder();
            reply.PushInt32(xid);
            reply.PushInt32((int)msg_type.REPLY);
            reply.PushInt32((int)reply_stat.MSG_ACCEPTED);
            //The first field is an authentication verifier that the server generates in order to validate itself to the client.
            ServerCredential(reply);
            reply.PushInt32((int)reason);
            return reply;
        }
        RpcMsgBuilder RejectReply(int xid, reject_stat reason)
        {
            var reply = new RpcMsgBuilder();
            reply.PushInt32(xid);
            reply.PushInt32((int)msg_type.REPLY);
            reply.PushInt32((int)reply_stat.MSG_DENIED);            
            reply.PushInt32((int)reason);
            return reply;
        }
        RpcMsgBuilder RpcMismatchReply(int xid)
        {
            var reply = RejectReply(xid,reject_stat.RPC_MISMATCH);
            reply.PushInt32(2);		// rpc_msg.reply_body.rejected_reply.mismatch_info.low
            reply.PushInt32(2);		// rpc_msg.reply_body.rejected_reply.mismatch_info.low
            return reply;
        }
        RpcMsgBuilder ProgMismatchReply(int xid,int progId)
        {
            var reply = AcceptReply(xid,accept_stat.PROG_MISMATCH);
            reply.PushInt32(progId);		// rpc_msg.reply_body.rejected_reply.mismatch_info.low
            reply.PushInt32(progId);		// rpc_msg.reply_body.rejected_reply.mismatch_info.low
            return reply;
        }
        
    }
}
