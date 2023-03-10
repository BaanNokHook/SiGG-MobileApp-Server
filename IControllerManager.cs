// Satellite-Communication-Server //

using SocketAppServer.CoreServices.ControllerManagement;
using SocketAppServer.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface IControllerManager
    {
        void RegisterController(Type type);

        void RegisterAllControllers(Assembly assembly, string namespaceName);

        ControllerRegister GetControllerRegister(string controllerName);
        IController InstantiateController(string controllerName, RequestBody requestBody);
        IReadOnlyCollection<ControllerRegister> GetRegisteredControllers();
    }
}
