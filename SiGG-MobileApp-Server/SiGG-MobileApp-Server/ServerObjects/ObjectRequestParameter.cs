using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    internal class ObjectRequestParameter
    {
        public string Alias { get; private set; }
        public string TypeName { get; private set; }

        public ObjectRequestParameter(string alias, string name)
        {
            Alias = alias;
            TypeName = name;
        }
    }
}
