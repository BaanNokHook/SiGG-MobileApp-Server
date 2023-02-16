using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public interface IActionExceptionHandler
    {
        OperationResult Handle(Exception exception, SocketRequest request);
    }
}
