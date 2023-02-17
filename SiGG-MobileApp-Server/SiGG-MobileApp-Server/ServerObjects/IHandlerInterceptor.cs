using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public interface IHandlerInterceptor
    {
        string ControllerName { get; }

        string ActionName { get; }

        InterceptorHandleResult PreHandle(SocketRequest socketRequest);
    }
}
