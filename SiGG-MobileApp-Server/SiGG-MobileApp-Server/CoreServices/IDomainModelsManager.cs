// Sattelite-Communication-Server //

using SocketAppServer.CoreServices.DomainModelsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface IDomainModelsManager
    {
        void RegisterAllModels(Assembly assembly, string namespaceName);
        void RegisterModelType(Type modelType);
        ModelRegister GetModelRegister(string typeName);
    }
}
