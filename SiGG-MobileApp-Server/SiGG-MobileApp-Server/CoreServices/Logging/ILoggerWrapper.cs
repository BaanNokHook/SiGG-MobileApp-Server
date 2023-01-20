// Sattelite-Communication-Server //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SocketAppServer.CoreServices.Logging
{
    public interface ILoggerWrapper
    {
        void Register(ref ServerLog log);

        List<ServerLog> List(Expression<Func<ServerLog, bool>> query);
    }
}
