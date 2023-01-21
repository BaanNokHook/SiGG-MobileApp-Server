// Satellite-Communication-Server //

using SocketAppServer.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices.DIManagement
{
    internal class DependencyInjectorManagerImpl : IDependencyInjectionService
    {
        private List<IDependencyInjectorMaker> dependencyInjectors = null;

        public DependencyInjectorManagerImpl()
        {
            dependencyInjectors = new List<IDependencyInjectorMaker>(10);
        }

        public void AddDependencyInjector(IDependencyInjectorMaker injectorMaker)
        {
            if (string.IsNullOrEmpty(injectorMaker.ControllerName))
                throw new Exception("It is no longer possible to use empty strings or nulls for ControllerName. If your intention is to indicate ALL, use the '*' character inside the string");
            dependencyInjectors.Add(injectorMaker);
        }

        public IDependencyInjectorMaker GetInjectorMaker(string controllerName)
        {
            IDependencyInjectorMaker controllerMaker = dependencyInjectors.FirstOrDefault(di => di.ControllerName.Equals(controllerName));
            if(controllerMaker == null)
                controllerMaker = dependencyInjectors.FirstOrDefault(di => di.ControllerName.Equals("*"));
            return controllerMaker;
        }
    }
}
