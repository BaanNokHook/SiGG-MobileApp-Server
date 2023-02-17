using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public class InterceptorHandleResult
    {
        internal bool CancelActionInvoke { get; set; }
        public bool ResponseSuccess { get; }
        internal string Message { get; set; }

        internal object Data { get; set; }

        public InterceptorHandleResult(bool cancelActionInvoke,
            bool responseSuccess,
            string message, object data)
        {
            CancelActionInvoke = cancelActionInvoke;
            ResponseSuccess = responseSuccess;
            Message = message;
            Data = data;
        }
    }
}
