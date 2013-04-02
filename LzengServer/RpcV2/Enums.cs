using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzengServer.RpcV2
{
    enum msg_type
    {
        CALL = 0,
        REPLY = 1
    };
    enum reply_stat
    {
        MSG_ACCEPTED = 0,
        MSG_DENIED = 1
    };
    enum accept_stat
    {
        SUCCESS = 0, /* RPC executed successfully */
        PROG_UNAVAIL = 1, /* remote hasn’t exported program */
        PROG_MISMATCH = 2, /* remote can’t support version # */
        PROC_UNAVAIL = 3, /* program can’t support procedure */
        GARBAGE_ARGS = 4, /* procedure can’t decode params */
        SYSTEM_ERR = 5 /* errors like memory allocation failure */
    };
    enum reject_stat
    {
        RPC_MISMATCH = 0, /* RPC version number != 2 */
        AUTH_ERROR = 1 /* remote can’t authenticate caller */
    };
    enum auth_stat {
        AUTH_OK = 0, /* success */
        /*
        * failed at remote end
        */
        AUTH_BADCRED = 1, /* bad credential (seal broken) */
        AUTH_REJECTEDCRED = 2, /* client must begin new session */
        AUTH_BADVERF = 3, /* bad verifier (seal broken) */
        AUTH_REJECTEDVERF = 4, /* verifier expired or replayed */
        AUTH_TOOWEAK = 5, /* rejected for security reasons */
        /*
        * failed locally
        */
        AUTH_INVALIDRESP = 6, /* bogus response verifier */
        AUTH_FAILED = 7 /* reason unknown */
    };
    enum auth_flavor
    {
        AUTH_NONE = 0,
        AUTH_SYS = 1,
        AUTH_SHORT = 2
        /* and more to be defined */
    };
}
