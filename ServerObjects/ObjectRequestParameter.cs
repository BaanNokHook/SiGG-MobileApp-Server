// Satellite-Communication-Server //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.ServerObjects
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
