using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.CoreServer
{
    public class LockedActionException : Exception
    {
        public LockedActionException(string message) : base(message)
        {
        }
    }
}