using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public class RequestParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        internal RequestParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        internal bool IsComplexType()
        {
            return Name.Contains(".");
        }

        public override string ToString()
        {
            return Name;
        }

        internal string GetAliasName()
        {
            int pIndex = Name.IndexOf('.');
            int ptIndex = pIndex + 1;
            string aliasName = Name.Substring(0, pIndex);
            return aliasName;
        }

        internal string GetParameterProperyName()
        {
            int pIndex = Name.IndexOf('.');
            int ptIndex = pIndex + 1;
            string propertyName = Name.Substring(ptIndex, Name.Length - ptIndex);
            return propertyName;
        }

        public RequestParameter()
        {

        }
    }
}
