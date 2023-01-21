// DT Software //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.CoreServices.DomainModelsManagement
{
    public class ModelRegister
    {
        public string ModeName { get; internal set; }
        public Type ModelType { get; internal set; }

        internal ModelRegister(string name, Type type)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name cannot be null");
            if (type == null)
                throw new Exception("Type cannot be null");

            ModeName = name;
            ModelType = type;
        }
    }
}
