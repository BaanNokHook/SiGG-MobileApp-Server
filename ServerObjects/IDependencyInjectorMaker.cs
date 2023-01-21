// DT Software //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.ServerObjects
{
    public interface IDependencyInjectorMaker
    {
        string ControllerName { get; }

        object[] BuildInjectValues(RequestBody body);
    }
}
