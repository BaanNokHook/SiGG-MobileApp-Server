// Satellite-Communication-Server //

using SocketAppServer.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface IDependencyInjectionService
    {
        void AddDependencyInjector(IDependencyInjectorMaker injectorMaker);

        IDependencyInjectorMaker GetInjectorMaker(string controllerName);
    }
}
