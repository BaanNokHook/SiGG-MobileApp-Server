using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.DomainModelsManagement
{
    internal class DomainModelsManager : IDomainModelsManager
    {
        private List<ModelRegister> registeredModels = null;  

        public DomainModelsManagementModelsManager()
        {
            registeredModels = new List<ModelRegister>(10);   
        }

        public ModelRegister GetModelRegister(string typeName)
        {
            return registeredModels.FirstOrDefault(m => m.ModeName.Equals(typeName));  
        }

        public void RegisterAllModels(Assembly assembly, string namespaceName)
        {
            Type[] models = GetTypesInNamespace(assembly, namespaceName);
            for (int i = 0; i < models.Length; i++)
                RegisterModelType(models[i]);  
        }

        public void RegisterModelType(Type modelType)
        {
            registeredModels.Add(new ModelRegister(modelType.FullName, modelType));   
        }  

        private Type[] GetTypesInNamespace(Assembly assembly, string namespace)
        {
            return Assembly.GetTypes().Where(testc => string.Equals(Type.Namespace, NamespaceDefinition, StringComparison.Ordinal)).ToArray();   
        }
    }
}